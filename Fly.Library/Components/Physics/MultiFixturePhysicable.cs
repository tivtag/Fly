// <copyright file="MultiFixturePhysicable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.MultiFixturePhysicable class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using Atom;
    using Atom.Math;
    using Atom.Xna;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Dynamics.Contacts;
    using Fly.Entities;
    using Fly.Physics.Descriptors;
    using Fly.Saving;

    /// <summary>
    /// Adds a physics body that is made out of multiple <see cref="Fixture"/>s to the entity.
    /// </summary>
    public sealed class MultiFixturePhysicable : BasePhysicable, ICollideable, IIntersectable, IScaleable, ISaveable
    {
        public event RelaxedEventHandler<ICollideable, IFlyEntity> Collided;

        public IMultiPhysicsBodyDescription FixtureDescription
        {
            get;
            set;
        }

        public IEnumerable<Fixture> Fixtures
        {
            get 
            {
                return fixtures; 
            }

            set
            {
                this.fixtures = value.ToArray();
            }
        }
        
        protected override void Generate()
        {
            DeletePhysicsObjects();

            var data = this.FixtureDescription.Build( this.PhysicsWorld );

            this.Fixtures = data.Item1;
            this.body = data.Item2;
            this.FixtureData = data.Item3;
            
            foreach( Fixture fixture in this.fixtures )
            {
                fixture.OnCollision = this.OnCollision;
                fixture.UserData = this;      
            }
            
            this.body.UserData = this;
            this.ReintegrateEntityTransform();
        }

        private void DeletePhysicsObjects()
        {
            DeleteBody();
            DeleteBreakableBody();
            DeleteFixtures();

            PhysicsWorld.ProcessChanges();
        }

        private void DeleteBody()
        {
            if( this.body != null )
            {
                this.body.UserData = null;
                this.PhysicsWorld.RemoveBody( this.body );
                this.body = null;
            }
        }

        private void DeleteBreakableBody()
        {
            var breakableBody = this.FixtureData as BreakableBody;

            if( breakableBody != null )
            {
                foreach( var body in breakableBody.DecomposedBodies )
                {
                    this.PhysicsWorld.RemoveBody( body );
                }

                this.PhysicsWorld.RemoveBreakableBody( breakableBody );
                this.FixtureData = null;
            }
        }

        private void DeleteFixtures()
        {
            if( this.fixtures != null )
            {
                foreach( Fixture fixture in this.fixtures )
                {
                    fixture.OnCollision = null;
                    fixture.UserData = 1;
                }

                this.fixtures = null;
            }
        }

        public override Atom.Math.RectangleF GetBounds()
        {
            if( this.fixtures == null )
            {
                return new Atom.Math.RectangleF();
            }

            var trans = new FarseerPhysics.Common.Transform();
            Fixture fixture = this.fixtures[0];
            fixture.Body.GetTransform( out trans );

            FarseerPhysics.Collision.AABB fullAabb;
            fixture.Shape.ComputeAABB( out fullAabb, ref trans, 0 );

            for( int i = 1; i < this.fixtures.Count; ++i )
            {
                fixture = this.fixtures[i];
                fixture.Body.GetTransform( out trans );

                FarseerPhysics.Collision.AABB aabb;
                fixture.Shape.ComputeAABB( out aabb, ref trans, 0 );

                fullAabb.Combine( ref aabb );
            }

            var corner = fullAabb.LowerBound;
            var extends = fullAabb.Extents;
            return new Atom.Math.RectangleF( corner.X, corner.Y, extends.X * 2.0f, extends.Y * 2.0f );
        }

        public override void PostUpdate( IFlyUpdateContext updateContext )
        {
            if( this.body.FixtureList != null )
            {
                var physicsTransform = this.PhysicsTransform;
                this.Transform.Position = physicsTransform.Position.ToAtom();
                this.Transform.Rotation = physicsTransform.Angle;
            }
        }

        private bool OnCollision( Fixture a, Fixture b, Contact contact )
        {
            var component = (IFlyComponent)b.UserData;
            var target = component != null ? component.Owner : null;

            this.Collided.Raise( this, target );
            return true;
        }

        public bool IntersectsAt( Atom.Math.Vector2 point )
        {
            var p = point.ToXna();

            for( int i = 0; i < this.fixtures.Count; ++i )
            {
                if( fixtures[i].TestPoint( ref p ) )
                {
                    return true;
                }
            }

            return false;
        }

        public void ApplyForceInRandomDirection( float strength, RandMT rand )
        {
            foreach( Fixture fixture in this.fixtures )
            {
                float angle = rand.UncheckedRandomRange( 0.0f, Constants.TwoPi );
                Vector2 force = Vector2.FromAngle( angle ) * strength;

                fixture.Body.ApplyForce( force.ToXna() );                
            }
        }

        public void ScaleTo( Atom.Math.Vector2 factor )
        {
            var desc = this.FixtureDescription as IScaleable;

            if( desc != null )
            {
                desc.ScaleTo( factor );
                this.Generate();
            }
        }

        public void Serialize( Atom.Storage.ISerializationContext context )
        {
            context.WriteDefaultHeader();
            context.WriteObject( this.FixtureDescription );
        }

        public void Deserialize( Atom.Storage.IDeserializationContext context )
        {
            context.ReadDefaultHeader( this.GetType() );
            this.FixtureDescription = context.ReadObject<IMultiPhysicsBodyDescription>();
        }

        private IList<Fixture> fixtures;
    }
}
