// <copyright file="EnemyFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Enemies.EnemyFactory class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Enemies
{
    using Atom.Math;
    using Fly.Empires;
    using Fly.Enemies.Behaviours;
    using Fly.Graphics.Strategies;
    using Fly.Physics.Descriptors;

    /// <summary>
    /// Builds new specific Enemy entity instance.
    /// </summary>
    public static class EnemyFactory
    {
        public static KamikazeEnemy KamikazeBlob( Vector2 position, Empire enemyEmpire, Empire targetEmpire, float radius = 0.5f )
        {
            KamikazeEnemy enemy = new KamikazeEnemy();
            enemy.Behaveable.AddBehaviour( new KamikazeAttackEnemyBehaviour() { Target = targetEmpire.HomePlanet } );

            enemy.Position = position;

            enemy.LifeStatus.LifePoints = 1;
            enemy.Damaging.Damage = 2;

            enemy.DrawStrategy = new TwoCircleDrawStrategy() {
                Radius = radius
            };

            enemy.Physics.FixtureDescription = new CircleDescription() {
                Radius = radius,
                Density = 10.0f
            };
            
            return enemy;
        }
    }
}
