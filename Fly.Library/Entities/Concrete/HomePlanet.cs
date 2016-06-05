// <copyright file="HomePlanet.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.Concrete.HomePlanet class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities
{
    using Fly.Components;
    using Fly.Entities.Basic;

    /// <summary>
    /// Represents the home planet of an empire. The home planet gets destroyed by damage.
    /// </summary>
    public sealed class HomePlanet : SingleGravityPhysicsEntity, IBehavedEntity
    {
        public LifeStatusComponent LifeStatus
        {
            get
            {
                return this.lifeStatus;
            }
        }

        public EmpireOwned OwnedBy
        {
            get
            {
                return this.owned;
            }
        }

        public Behaveable Behaveable
        {
            get 
            { 
                return behaveable; 
            }
        }

        public HomePlanet()
        {
            this.Components.BeginSetup();
            {
                this.Components.Add( this.lifeStatus );
                this.Components.Add( this.owned );
                this.Components.Add( this.behaveable );
            }
            this.Components.EndSetup();
        }

        private readonly LifeStatusComponent lifeStatus = new LifeStatusComponent();
        private readonly EmpireOwned owned = new EmpireOwned();
        private readonly Behaveable behaveable = new Behaveable();
    }
}
