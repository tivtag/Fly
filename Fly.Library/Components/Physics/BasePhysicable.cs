// <copyright file="BasePhysicable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.BasePhysicable class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Atom;
    using Atom.Math;
    using Atom.Xna;
    using FarseerPhysics.Dynamics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Represents the abstract base class for all components that add physics behaviour to an entity.
    /// </summary>
    public abstract class BasePhysicable : FlyComponent, IPhysicable, IBoundCalculator
    {
        public event SimpleEventHandler<IPhysicable> Generated;

        public Body Body
        {
            get
            {
                return this.body;
            }
        }

        public float Mass
        {
            get
            {
                return this.body.Mass;
            }
        }

        public Vector2 LinearVelocity
        {
            get
            {
                return this.body.LinearVelocity.ToAtom();
            }

            set
            {
#if DEBUG_NAN
                if( float.IsNaN( value.X ) || float.IsNaN( value.Y ) )
                {
                    throw new System.ArgumentNullException( "can't apply NaN lf" );
                }
#endif

                this.body.LinearVelocity = value.ToXna();
            }
        }

        public Vector2 Center
        {
            get
            {
                return this.body.WorldCenter.ToAtom();
            }
        }

        public Transformable Transform
        {
            get
            {
                return this.Owner.Transform;
            }
        }

        public World PhysicsWorld
        {
            get
            {
                return this.Scene.PhysicsWorld;
            }
        }

        public bool IsStatic
        {
            get
            {
                return this.body.IsStatic;
            }

            set
            {
                this.body.IsStatic = value;
            }
        }

        public float AngularDamping
        {
            get
            {
                return this.body.AngularDamping;
            }

            set
            {
                this.body.AngularDamping = value;
            }
        }

        public float LinearDamping
        {
            get
            {
                return this.body.LinearDamping;
            }

            set
            {
                this.body.LinearDamping = value;
            }
        }

        public FarseerPhysics.Common.Transform PhysicsTransform
        {
            get
            {
                FarseerPhysics.Common.Transform transform;
                this.body.GetTransform( out transform );
                return transform;
            }
        }
        
        public Atom.Math.Vector2 PhysicsPosition
        {
            get 
            {
                return this.body.Position.ToAtom();
            }
        }

        public float AngularVelocity
        {
            get
            {
                return this.body.AngularVelocity;
            }

            set
            {
                this.body.AngularVelocity = value;
            }
        }
 
        public object FixtureData 
        {
            get;
            protected set;
        }

        public override void Initialize()
        {
            this.Owner.Added += this.OnOwnerAdded;
            this.Owner.Removed += this.OnOwnerRemoved;
            base.Initialize();
        }
                
        private void OnOwnerRemoved( object sender, FlyScene scene )
        {
            if( this.body != null )
            {
                scene.PhysicsWorld.RemoveBody( this.body );
                this.body = null;
            }

            this.OnOwnerRemoved( scene );
        }

        protected virtual void OnOwnerRemoved( FlyScene scene )
        {
        }

        private void OnOwnerAdded( object sender, FlyScene scene )
        {
            this.GenerateCore();
        }

        private void GenerateCore()
        {
            this.Generate();
            this.Generated.Raise( this );
        }
        protected abstract void Generate();

        public override void InitializeBindings()
        {
            this.Transform.PositionChanged += this.OnEntityPositionChanged;
            this.Transform.RotationChanged += this.OnEntityRotationChanged;
        }

        private void OnEntityPositionChanged( object sender, Atom.ChangedValue<Vector2> e )
        {
            if( this.body == null )
                return;

            var physicsPosition = this.body.Position;
            var entityPosition = e.NewValue;

            if( physicsPosition.X != entityPosition.X || 
                physicsPosition.Y != entityPosition.Y )
            {
                this.body.Position = entityPosition.ToXna();
            }
        }

        private void OnEntityRotationChanged( object sender, Atom.ChangedValue<float> e )
        {
            if( this.body == null )
                return;

            if( this.body.Rotation != e.NewValue )
            {
                this.body.Rotation = e.NewValue;
            }
        }

        public void ApplyForce( Vector2 force )
        {
#if DEBUG_NAN
            if( float.IsNaN( force.X ) || float.IsNaN( force.Y ) )
            {
                throw new System.ArgumentNullException( "can't apply NaN force" );
            }
#endif

            this.body.ApplyForce( force.ToXna() );
        }

        public void ApplyTorque( float torque )
        {
#if DEBUG_NAN
            if( float.IsNaN( torque ) )
            {
                throw new System.ArgumentNullException( "can't apply NaN torque" );
            }
#endif

            this.body.ApplyTorque( torque );
        }

        protected void ReintegrateEntityTransform()
        {
            var position = new Xna.Vector2( this.Transform.X, this.Transform.Y );

            if( body.FixtureList != null )
            {
                this.body.SetTransformIgnoreContacts( ref position, this.Transform.Rotation );
            }
        }

        public abstract Atom.Math.RectangleF GetBounds();

        protected Body body;
    }
}
