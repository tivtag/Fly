// <copyright file="SingleGravityPhysicsEntity.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.Basic.SingleGravityPhysicsEntity class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities.Basic
{
    using Fly.Components;

    /// <summary>
    /// Represents a <see cref="FlyEntity"/> that has a single physical body and can emit/receive gravity.
    /// </summary>
    public class SingleGravityPhysicsEntity : FlyEntity, IGravityReceivingEntity, IGravityEmittingEntity
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

        public GravityReceiving GravityReceiver
        {
            get 
            {
                return this.gravityReceiver;
            }
        }

        public GravityEmitting GravityEmitter
        {
            get 
            {
                return this.gravityEmitter;
            }
        }

        public SingleGravityPhysicsEntity()
        {
            this.Components.BeginSetup();
            {
                this.Components.Add( this.physics );
                this.Components.Add( this.gravityEmitter );
                this.Components.Add( this.gravityReceiver );
            }
            this.Components.EndSetup();
        }

        private readonly SingleFixturePhysicable physics = new SingleFixturePhysicable();
        private readonly GravityEmitting gravityEmitter = new GravityEmitting();
        private readonly GravityReceiving gravityReceiver = new GravityReceiving();
    }
}
