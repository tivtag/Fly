// <copyright file="Ship.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.Concrete.Ship class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities.Concrete
{
    using Fly.Components;
    using Fly.Entities.Basic;

    /// <summary>
    /// Represents a ship (for example the player ship) that can gather things, can get destroyed and is owned by an empire.
    /// </summary>
    public sealed class Ship : SingleGravityPhysicsEntity, IGatheringEntity
    {
        public Gathering Gathering
        {
            get
            {
                return this.gathering;
            }
        }

        public EmpireOwned OwnedBy
        {
            get
            {
                return this.empireOwned;
            }
        }

        public LifeStatusComponent LifeStatus
        {
            get
            {
                return this.lifeStatus;
            }
        }

        public Ship()
        {
            this.Components.BeginSetup();
            {
                this.Components.Add( this.gathering );
                this.Components.Add( this.empireOwned );
                this.Components.Add( this.lifeStatus );
            }
            this.Components.EndSetup();
        }

        private readonly Gathering gathering = new Gathering();
        private readonly LifeStatusComponent lifeStatus = new LifeStatusComponent();
        private readonly EmpireOwned empireOwned = new EmpireOwned();
    }
}
