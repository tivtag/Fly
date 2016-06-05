// <copyright file="LifeStatusComponent.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.LifeStatusComponent class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Atom;
    using Fly.Entities;
    using Fly.Saving;

    /// <summary>
    /// Adds a life/health status to an entity; allowing it to be damaged by other <see cref="IDamageEntity"/>s.
    /// When the life status reaches zero the entity gets destroyed and might break into multiple parts if it is Breakable.
    /// </summary>
    public sealed class LifeStatusComponent : FlyComponent, ILifeStatusComponent, ISaveable
    {
        private const int DefaultMaximumLifePoints = 3;
        public event SimpleEventHandler<LifeStatusComponent> Destroyed;

        public int LifePoints
        {
            get
            {
                return this.lifePoints;
            }

            set
            {
                this.lifePoints = value;
            }
        }

        public int MaximumLifePoints
        {
            get
            {
                return this.maximumLifePoints;
            }

            set
            {
                this.maximumLifePoints = value;
            }
        }

        public override void InitializeBindings()
        {
            this.collideable = this.Owner.Components.Find<ICollideable>();

            this.collideable.Collided += OnCollided;
        }

        public void Refill()
        {
            this.lifePoints = this.maximumLifePoints;
        }

        private void OnCollided( ICollideable sender, IFlyEntity other )
        {
            var damageEntity = other as IDamageEntity;

            if( damageEntity != null )
            {
                this.OnHit( damageEntity.Damaging );
            }
        }

        private void OnHit( Damaging damaging )
        {
            if( this.lifePoints > 0 )
            {
                this.lifePoints -= damaging.Damage;
                damaging.OnDamaged( this.Owner );

                if( this.lifePoints <= 0 )
                {
                    this.Destroy();
                }
            }
        }

        private void OnDestroyed()
        {
            this.Destroyed.Raise( this );

            var breakable = this.Owner.Components.Find<Breakable>();
            if( breakable != null )
            {
                breakable.Break();
            }
        }

        public void Destroy()
        {
            this.lifePoints = 0;
            this.OnDestroyed();
        }

        public void Serialize( Atom.Storage.ISerializationContext context )
        {
            context.WriteDefaultHeader();
            context.Write( this.maximumLifePoints );
        }

        public void Deserialize( Atom.Storage.IDeserializationContext context )
        {
            context.ReadDefaultHeader( this.GetType() );
            this.lifePoints = this.maximumLifePoints = context.ReadInt32();
        }

        private int lifePoints = DefaultMaximumLifePoints;
        private int maximumLifePoints = DefaultMaximumLifePoints;
        private ICollideable collideable;
    }
}
