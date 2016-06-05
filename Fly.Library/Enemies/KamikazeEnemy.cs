// <copyright file="KamikazeEnemy.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Enemies.KamikazeEnemy class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Enemies
{
    using Fly.Behaviours;
    using Fly.Components;
    using Fly.Entities;

    /// <summary>
    /// Represents a damaging entitiy that steadily moves towards a target.
    /// </summary>
    public sealed class KamikazeEnemy : BasicEnemy, IDamageEntity
    {
        public Damaging Damaging
        {
            get
            {
                return this.damaging;
            }
        }

        public KamikazeEnemy()
        {
            this.Components.BeginSetup();
            {
                this.Components.Add( this.damaging );
            }
            this.Components.EndSetup();

            this.damaging.DamagedOther += this.OnDamaged;
            this.LifeStatus.Destroyed += this.OnDestroyed;
        }

        private void OnDestroyed( LifeStatusComponent sender )
        {
            this.Despawn();
        }

        private void OnDamaged( Damaging sender, Entities.IFlyEntity e )
        {
            this.Despawn();
        }

        private void Despawn()
        {
            Behave.This( this )
                .BlendOut( forSeconds: 0.33f )
                .Despawn();
        }

        private readonly Damaging damaging = new Damaging();
    }
}
