// <copyright file="PowerUp.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.Concrete.PowerUp class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities.Concrete
{
    using Fly.Entities.Basic;
    using Fly.Components;

    /// <summary>
    /// Represents an entity that when picked up (usually) powers up the gatherer.
    /// </summary>
    public sealed class PowerUp : SingleGravityPhysicsEntity, IBehavedEntity
    {
        public Behaveable Behaveable
        {
            get
            {
                return this.behaveable;
            }
        }

        public Pickupable Pickupable
        {
            get
            {
                return this.pickupable;
            }
        }

        public PowerUp()
        {
            this.Components.BeginSetup();
            {
                this.Components.Add( this.behaveable );
                this.Components.Add( this.pickupable );
            }
            this.Components.EndSetup();
        }

        private readonly Pickupable pickupable = new Pickupable();
        private readonly Behaveable behaveable = new Behaveable();
    }
}
