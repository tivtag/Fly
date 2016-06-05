// <copyright file="KamikazeBlubWave.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Waves.KamikazeBlubWave class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Waves
{
    using System.Globalization;
    using Atom.Math;
    using Fly.Enemies;

    /// <summary>
    /// Implements an IWave that spawns many small enemies that move towards the home world of the player
    /// in a single kamikaze attack.
    /// </summary>
    /// <seealso cref="EnemyFactory.KamikazeBlob"/>
    public sealed class KamikazeBlubWave : IWave
    {
        private static readonly FloatRange BlubSize = new FloatRange( 0.4f, 0.6f );
        private static readonly FloatRange BlubGap  = new FloatRange( 0.4f, 0.6f );
        private static readonly IntegerRange DefaultEnemyCount = new IntegerRange( 50, 100 );
        
        public string ContentDescription
        {
            get 
            {
                return this.enemyCount.ToString( CultureInfo.CurrentCulture ) + " Kamikaze Eyes";
            }
        }

        public KamikazeBlubWave( IRand rand )
        {
            this.rand = rand;
            this.enemyCount = DefaultEnemyCount.GetRandomValue(this.rand);
        }

        public void SpawnIn( GameWorld world )
        {
            Direction4 direction = rand.RandomDirection4But( Direction4.None );

            this.SpawnIn( world, direction.ToVector() );
        }

        private void SpawnIn( GameWorld world, Vector2 slideDirection )
        {            
            Vector2 wabbleDirection = slideDirection.Perpendicular;
                        
            float length      = ((BlubSize.Maximum*2.0f) + BlubGap.Maximum) * enemyCount;
            Point2 worldSize = world.Scene.HalfSize;

            Vector2 position = (wabbleDirection * worldSize) * 0.95f + slideDirection * new Vector2( 
                rand.RandomRange( BlubSize.Maximum, worldSize.X - length ), 
                rand.RandomRange( BlubSize.Maximum, worldSize.Y - length ) 
            );

            for( int i = 0; i < enemyCount; ++i )
            {
                float radius = BlubSize.GetRandomValue( this.rand );
                this.SpawnBlub( position, radius, world );
                
                float positionOffset = (2 * radius) + BlubGap.GetRandomValue( this.rand );
                position += slideDirection * positionOffset;               
            }
        }

        private void SpawnBlub( Vector2 position, float radius, GameWorld world )
        {
            var blob = EnemyFactory.KamikazeBlob( position, world.EnemyEmpire, world.PlayerEmpire, radius );
            world.AddEntity( blob );            
        }

        private readonly IRand rand;
        private readonly int enemyCount;
    }
}
