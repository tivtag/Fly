// <copyright file="KamikazeAttackEnemyBehaviour.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Enemies.Behaviours.KamikazeAttackEnemyBehaviour class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Enemies.Behaviours
{
    using Atom.Math;
    using Fly.Entities;

    /// <summary>
    /// Implements an <see cref="EnemyBehaviour"/> that steadily moves the enemy entity towards a target entity.
    /// </summary>
    public sealed class KamikazeAttackEnemyBehaviour : EnemyBehaviour
    {
        public FlyEntity Target
        {
            get;
            set;
        }

        public override void Update( IFlyUpdateContext updateContext )
        {
            if( this.Target == null )
            {
                return;
            }

            Vector2 delta = this.Target.Position - this.Entity.Position;
            Vector2 direction = delta.Direction;
            Vector2 force = direction * 0.5f;

            this.Entity.Physics.ApplyForce( force );
        }
    }
}
