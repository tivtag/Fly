// <copyright file="GameWorld.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.GameWorld class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using Atom.Math;
    using Fly.Empires;
    using Fly.Entities;
    using Fly.Entities.Concrete;
    using Fly.Physics;

    /// <summary>
    /// Aggregates the objects that make up the whole simulated game world.
    /// </summary>
    public sealed class GameWorld
    {
        public FlyScene Scene
        {
            get
            {
                return this.scene;
            }
        }

        public Ship Player
        {
            get;
            set;
        }

        public Camera Camera
        {
            get;
            set;
        }

        public Empire EnemyEmpire
        {
            get
            {
                return this.enemyEmpire;
            }
        }

        public Empire PlayerEmpire
        {
            get
            {
                return this.playerEmpire;
            }
        }

        public static GameWorld Create( FlyScene scene, IRand rand )
        {
            var playerEmpire = new Empire();
            var enemyEmpire = new Empire();
            return new GameWorld( playerEmpire, enemyEmpire, scene, rand );
        }

        public GameWorld( Empire playerEmpire, Empire enemyEmpire, FlyScene scene, IRand rand )
        {
            this.playerEmpire = playerEmpire;
            this.enemyEmpire = enemyEmpire;
            this.scene = scene;
            this.gravity = new SceneGravityApplicator( scene );
        }

        public void RemoveEntity( IFlyEntity entity )
        {
            this.scene.RemoveEntity( entity );
        }

        public void AddEntity( IFlyEntity entity )
        {
            this.scene.AddEntity( entity );
        }

        public void Update( IFlyUpdateContext updateContext )
        {
            this.gravity.Apply();
            this.scene.Update( updateContext );
        }        

        private readonly Empire enemyEmpire;
        private readonly Empire playerEmpire;
        private readonly FlyScene scene;
        private readonly SceneGravityApplicator gravity;
    }
}
