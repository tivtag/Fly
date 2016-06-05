// <copyright file="BasicEnemy.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Enemies.BasicEnemy class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Enemies
{
    using Fly.Components;
    using Fly.Entities;

    /// <summary>
    /// Provides the abstract base class for enemy entities of the player faction.
    /// </summary>
    public abstract class BasicEnemy : FlyEntity, IBehavedEntity, IGravityReceivingEntity
    {
        public SingleFixturePhysicable Physics
        {
            get
            {
                return this.physics;
            }
        }

        IPhysicable IPhysicsEntity.Physics
        {
            get
            {
                return this.physics;
            }
        }

        public Behaveable Behaveable
        {
            get
            {
                return this.behaveable;
            }
        }

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
                return this.ownedBy;
            }
        }

        public GravityReceiving GravityReceiver
        {
            get
            {
                return this.gravityReceiver;
            }
        }

        public BasicEnemy()
        {
            this.Components.BeginSetup();
            {
                this.Components.Add( this.behaveable );
                this.Components.Add( this.lifeStatus );
                this.Components.Add( this.ownedBy );
                this.Components.Add( this.physics );
                this.Components.Add( this.gravityReceiver );
            }
            this.Components.EndSetup();
        }

        private readonly SingleFixturePhysicable physics = new SingleFixturePhysicable();
        private readonly GravityReceiving gravityReceiver = new GravityReceiving();
        private readonly Behaveable behaveable = new Behaveable();
        private readonly EmpireOwned ownedBy = new EmpireOwned();
        private readonly LifeStatusComponent lifeStatus = new LifeStatusComponent();
    }
}
