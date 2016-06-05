// <copyright file="Asteroid.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.Concrete.Asteroid class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities.Concrete
{
    using Atom.Math;
    using Atom.Storage;
    using Fly.Behaviours;
    using Fly.Components;
    using Fly.Graphics;
    using Fly.Saving;

    /// <summary>
    /// Represents an Asteroid that when broken by damage splits into multiple parts and relases Minerals.
    /// </summary>
    public class Asteroid : FlyEntity, IBehavedEntity, IBreakableEntity, IPhysicsEntity, IGravityReceivingEntity, IDescribed
    {
        public float DespawnTime
        {
            get;
            set;
        }

        public string Description
        {
            get
            {
                int points = this.Destroyable.MaximumLifePoints;
                return points + "-class Asteroid";
            }
        }

        public MultiFixturePhysicable Physics
        {
            get
            {
                return this.physics;
            }
        }

        public Breakable Breakable
        {
            get
            {
                return this.breakable;
            }
        }

        public LifeStatusComponent Destroyable
        {
            get
            {
                return destroyable; 
            }
        }

        public Behaveable Behaveable
        {
            get
            {
                return this.behaveable;
            }
        }

        IPhysicable IPhysicsEntity.Physics
        {
            get 
            {
                return this.physics;
            }
        }

        public GravityReceiving GravityReceiver
        {
            get
            {
                return this.gravityReceiver;
            }
        }

        public Asteroid()
        {
            this.Components.BeginSetup();
            {
                this.Components.Add( this.physics );
                this.Components.Add( this.gravityReceiver );
                this.Components.Add( this.breakable );
                this.Components.Add( this.destroyable );
                this.Components.Add( this.behaveable );
            }
            this.Components.EndSetup();
            this.HookEvents();

            this.DespawnTime = 5.0f;
            this.gravityReceiver.Factor = 0.65f;
        }

        private void HookEvents()
        {
            this.breakable.Broken += this.OnBroken;
        }

        private void OnBroken( Breakable sender )
        {
            var asteroid = this;
            
            Behave.This( asteroid )
                .After( DespawnTime )
                    .BlendOut( forSeconds: 3 )
                    .Despawn()
                .AndAtTheSameTime()
                .Spawn( CreateMineral, asteroid.Destroyable.MaximumLifePoints / 3 );
        }

        private static IFlyEntity CreateMineral( Asteroid a )
        {
            var mineral = EntityFactory.CreateMineral();
            mineral.Position = a.Physics.PhysicsPosition;

            Behave.This( mineral )
                .BlendIn( forSeconds: 1 )
                .AndAtTheSameTime()
                .After( seconds: 10 )
                .BlendOut( forSeconds: 2 )
                .Despawn();

            return mineral;
        }

        public override void Serialize( ISerializationContext context )
        {
            context.WriteDefaultHeader();

            context.Write( this.Position );
            context.Write( this.Rotation );

            context.WriteObject( this.DrawStrategy );
            this.Physics.Serialize( context );
            this.Destroyable.Serialize( context );
        }

        public override void Deserialize( IDeserializationContext context )
        {
            context.ReadDefaultHeader( typeof( Wall ) );
            this.Position = context.ReadVector2();
            this.Rotation = context.ReadSingle();

            this.DrawStrategy = context.ReadObject<IEntityDrawStrategy>();
            this.Physics.Deserialize( context );
            this.Destroyable.Deserialize( context );
        }

        private readonly GravityReceiving gravityReceiver = new GravityReceiving();
        private readonly MultiFixturePhysicable physics = new MultiFixturePhysicable();
        private readonly Breakable breakable = new Breakable();
        private readonly LifeStatusComponent destroyable = new LifeStatusComponent();
        private readonly Behaveable behaveable = new Behaveable();
    }
}