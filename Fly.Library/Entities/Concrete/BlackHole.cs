// <copyright file="BlackHole.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.Concrete.BlackHole class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities.Concrete
{
    using Fly.Components;

    /// <summary>
    /// Represents an unmoveable black hole that strongly attracts gravity receiving entities.
    /// </summary>
    public sealed class BlackHole : FlyEntity, IBehavedEntity, IGravityEmittingEntity
    {    
        public GravityEmitting GravityEmitter
        {
            get 
            {
                return this.gravityEmitting;   
            }
        }
        
        public Behaveable Behaveable
        {
            get 
            {
                return this.behaveable;
            }
        }

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
        
        public BlackHole()
        {
            this.Components.BeginSetup();
            this.Components.Add( this.physics );
            this.Components.Add( this.gravityEmitting );
            this.Components.Add( this.behaveable );
            this.Components.EndSetup();
        }

        private readonly SingleFixturePhysicable physics = new SingleFixturePhysicable();
        private readonly GravityEmitting gravityEmitting = new GravityEmitting();
        private readonly Behaveable behaveable = new Behaveable();
    }
}
