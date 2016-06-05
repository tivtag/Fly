// <copyright file="Bullet.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.Concrete.Bullet class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities.Concrete
{
    using Fly.Entities.Basic;
    using Fly.Components;
    using Fly.Behaviours;

    /// <summary>
    /// Reprents a bullet that when damages an entity on impact.
    /// </summary>
    public sealed class Bullet : SingleGravityPhysicsEntity, IDamageEntity, IBehavedEntity
    {
        public Damaging Damaging
        {
            get 
            {
                return this.damaging;   
            }
        }
        
        public Behaveable Behaveable
        {
            get 
            {
                return this.behaveable;
            }
        }

        public Bullet( float activeTime )
        {
            this.Components.BeginSetup();
            this.Components.Add( this.damaging );
            this.Components.Add( this.behaveable );
            this.Components.EndSetup();

            Behave.This( this )
                .After( activeTime )
                .BlendOut( forSeconds: 1 )
                .Despawn();

            this.damaging.DamagedOther += this.OnDamageApplied;
        }

        private void OnDamageApplied( Damaging sender, IFlyEntity e )
        {
            // ToDo: Add bounce support!

            Behave.This( this )
                .BlendOut( forSeconds: 1 )
                .Despawn();
        }

        private readonly Damaging damaging = new Damaging();
        private readonly Behaveable behaveable = new Behaveable();
    }
}
