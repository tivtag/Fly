// <copyright file="EnemyBehaviour.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Enemies.Behaviours.EnemyBehaviour class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Enemies.Behaviours
{
    using Fly.Behaviours;

    /// <summary>
    /// Represents the <see cref="BaseBehaviour"/> base class for Enemy Entity behaviours.
    /// </summary>
    public abstract class EnemyBehaviour : BaseBehaviour
    {
        public new BasicEnemy Entity
        {
            get
            {
                return (BasicEnemy)base.Entity;
            }

            set
            {
                base.Entity= value;
            }
        }
    }
}
