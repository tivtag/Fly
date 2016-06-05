// <copyright file="SingleFixturePhysicable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.SingleFixturePhysicable class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Atom;
    using Atom.Xna;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Dynamics.Contacts;
    using Fly.Entities;
    using Fly.Physics.Descriptors;

    /// <summary>
    /// Adds a physics body that is made out a single <see cref="Fixture"/> to the entity.
    /// </summary>
    public sealed class SingleFixturePhysicable : BasePhysicable, ICollideable, IIntersectable, IScaleable
    {
        public event RelaxedEventHandler<ICollideable, IFlyEntity> Collided;

        public IPhysicsBodyDescription FixtureDescription
        {
            get
            {
                return this.fixtureDescriptor;
            }

            set
            {
                this.fixtureDescriptor = value;
            }
        }

        public Fixture Fixture
        {
            get
            {
                return this.fixture;
            }
        }

        public float Rotation
        {
            get
            {
                return this.fixture.Body.Rotation;
            }

            set
            {
                this.fixture.Body.Rotation = value;
            }
        }
        
        protected override void OnOwnerRemoved( FlyScene scene )
        {
            this.fixture = null;
        }

        protected override void Generate()
        {
            if( this.fixtureDescriptor != null )
            {
                if( this.body != null )
                {
                    this.body.UserData = null;
                    this.PhysicsWorld.RemoveBody( this.body );            
                }

                this.body = this.fixtureDescriptor.Build( this.PhysicsWorld );
                
                if( this.body != null )
                {
                    this.fixture = this.body.FixtureList[0];
                    
                    this.body.UserData = this;
                    this.ReintegrateEntityTransform();

                    this.fixture.OnCollision = this.OnCollision;
                    this.fixture.UserData = this;
                }
            }
        }

        private bool OnCollision( Fixture a, Fixture b, Contact contact )
        {
            var component = (IFlyComponent)b.UserData;
            var target = component != null ? component.Owner : null;

            this.Collided.Raise( this, target );
            return true;
        }

        public override void PostUpdate( IFlyUpdateContext updateContext )
        {
            var physicsTransform = this.PhysicsTransform;
            this.Transform.Position = physicsTransform.Position.ToAtom();
            this.Transform.Rotation = physicsTransform.Angle;
        }

        public override Atom.Math.RectangleF GetBounds()
        {
            if( this.fixture == null )
            {
                return new Atom.Math.RectangleF();
            }

            var trans = new FarseerPhysics.Common.Transform();     
            fixture.Body.GetTransform( out trans );

            FarseerPhysics.Collision.AABB aabb;
            fixture.Shape.ComputeAABB( out aabb, ref trans, 0 );

            var corner = aabb.LowerBound;
            var extends = aabb.Extents;
            return new Atom.Math.RectangleF( corner.X, corner.Y, extends.X * 2.0f, extends.Y * 2.0f );
        }

        public bool IntersectsAt( Atom.Math.Vector2 point )
        {
            if( fixture != null )
            {
                var p = point.ToXna();
                return fixture.TestPoint( ref p );
            }
            else
            {
                return false;
            }
        }

        public void LimitVelocityTo( float maximumVelocity )
        {
            this.LinearVelocity = Atom.Math.Vector2.Clamp(
                this.LinearVelocity,
                new Atom.Math.Vector2( -maximumVelocity, -maximumVelocity ),
                new Atom.Math.Vector2( maximumVelocity, maximumVelocity )
            );
        }

        public void ScaleTo( Atom.Math.Vector2 factor )
        {
            var desc = this.fixtureDescriptor as IScaleable;

            if( desc != null )
            {
                desc.ScaleTo( factor );
                this.Generate();
            }
        }

        private Fixture fixture;
        private IPhysicsBodyDescription fixtureDescriptor;
    }
}
