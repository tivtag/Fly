// <copyright file="Pickupable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.Pickupable class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Fly.Entities;
    using Fly.Items;

    /// <summary>
    /// Adds the mechanism of being able to be picked up by another <see cref="IGatheringEntity"/> to a FlyEntity.
    /// </summary>
    public sealed class Pickupable : FlyComponent
    {
        public IItem Item 
        {
            get;
            set;
        }

        public override void InitializeBindings()
        {
            var collideable = this.Owner.Components.Find<ICollideable>();

            collideable.Collided += this.OnCollided ;
        }

        private void OnCollided( ICollideable sender, Entities.IFlyEntity colidee )
        {
            var entity = colidee as IGatheringEntity;

            if( entity != null )
            {
                this.OnPickedUp( entity );
            }
        }

        private void OnPickedUp( IGatheringEntity entity )
        {
            if( entity.Gathering.Gather( this.Item ) )
            {
                if( this.Owner.Scene != null )
                {
                    this.Owner.Scene.RemoveEntity( this.Owner );
                }
            }
        }
    }
}
