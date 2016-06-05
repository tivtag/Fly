// <copyright file="TractorBeam.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.Concrete.TractorBeam class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities.Concrete
{
    using FarseerPhysics.Collision;
    using FarseerPhysics.Dynamics.Joints;
    using FarseerPhysics.Factories;
    using Xna = Microsoft.Xna.Framework;
    using Fly.Components;
    using Fly.Behaviours;

    /// <summary>
    /// Represents a beam that connects two entities via a visible binding force.
    /// For example that player ship can carry an astroid behind him with a tractor beam.
    /// </summary>
    public sealed class TractorBeam : FlyEntity, IBehavedEntity
    {
        public Joint Joint
        {
            get
            {
                return this.joint;
            }
        }

        public Behaveable Behaveable
        {
            get 
            {
                return this.behaveable;
            }
        }

        public TractorBeam( IPhysicsEntity from, IPhysicsEntity to )
        {
            this.from = from;
            this.to = to;

            this.Components.BeginSetup();
            {
                this.Components.Add( this.behaveable );
            }
            this.Components.EndSetup();

            this.HookEvents();
        }

        private void HookEvents()
        {
            this.from.Removed += this.OnTargetRemoved;
            this.to.Removed += this.OnTargetRemoved;

            this.HookOnDestroyed( this.to );
            this.HookOnDestroyed( this.from );
        }

        private void HookOnDestroyed( IFlyEntity entity )
        {
            var destroyable = entity.Components.Find<LifeStatusComponent>();

            if( destroyable != null )
            {
                destroyable.Destroyed += sender => this.RemoveFromScene();
            }
        }

        private void RemoveFromScene()
        {
            Behave.This( this )
                .BlendOut( forSeconds: 0.5f )
                .Despawn();
        }

        private void OnTargetRemoved( object sender, FlyScene e )
        {
            this.RemoveFromScene();
        }
        
        protected override void OnRemoved( FlyScene scene )
        {
            scene.PhysicsWorld.RemoveJoint( this.joint );
        }

        protected override void OnAdded( FlyScene scene )
        {
            float f = GetDistance( from );
            float t = GetDistance( to );

            float distance = f + t;
            float minWidth = distance;
            float maxWidth = distance + 6.0f;            

            this.joint = JointFactory.CreateSliderJoint( scene.PhysicsWorld, this.from.Physics.Body, this.to.Physics.Body, Xna.Vector2.Zero, Xna.Vector2.Zero, minWidth, maxWidth );

            this.joint.DampingRatio = 1.0f;
            this.joint.Frequency = 0.25f;
        }

        private float GetDistance( IPhysicsEntity entity )
        {
            var list = entity.Physics.Body.FixtureList;
      
            float maxDistance = -1.0f;

            if( list != null )
            {
                foreach( var fixture in list )
                {
                    AABB box;
                    fixture.GetAABB( out box, 0 );

                    float distance = box.Extents.Length();

                    if( distance > maxDistance )
                    {
                        maxDistance = distance;
                    }
                }
            }

            return maxDistance;
        }

        private readonly IPhysicsEntity from;
        private readonly IPhysicsEntity to;
        private SliderJoint joint;
        private readonly Behaveable behaveable = new Behaveable();
    }

}
