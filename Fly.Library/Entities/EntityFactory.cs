// <copyright file="EntityFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.EntityFactory class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities
{
    using Atom.Math;
    using FarseerPhysics.Dynamics;
    using Fly.Behaviours;
    using Fly.Empires;
    using Fly.Entities.Basic;
    using Fly.Entities.Concrete;
    using Fly.Graphics.Strategies;
    using Fly.Physics.Descriptors;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Builds new specific <see cref="IFlyEntity"/> instances.
    /// </summary>
    public static class EntityFactory
    {
        public static SingleGravityPhysicsEntity CreatePhysicsRectangle( float width, float height, float density, bool isStatic = false )
        {
            var entity = new SingleGravityPhysicsEntity();

            entity.DrawStrategy = new PhysicsShapeDrawStrategy();
            entity.Physics.FixtureDescription = new RectangleDescription() {
                Width = width,
                Height = height,
                Density = density,
                IsStatic = isStatic
            };

            return entity;
        }

        public static Wall CreateWall( float width, float height )
        {
            var entity = new Wall() {
                Name = "End of World"
            };

            entity.DrawStrategy = new PhysicsShapeDrawStrategy() {
                Color = Color.LightGreen
            };

            entity.Physics.FixtureDescription = new RectangleDescription() {
                Width = width,
                Height = height,
                Density = 1.0f,
                IsStatic = true
            };

            return entity;
        }

        public static SingleGravityPhysicsEntity CreatePhysicsCircle( float radius, float density, bool isStatic = false, int segmentCount = 32 )
        {
            var entity = new SingleGravityPhysicsEntity();

            entity.DrawStrategy = new PhysicsCirlceDrawStrategy() {
                SegmentCount = segmentCount
            };

            entity.Physics.FixtureDescription = new CircleDescription() {
                Radius = radius,
                Density = density,
                IsStatic = isStatic
            };

            return entity;
        }

        public static HomePlanet CreateHomePlanet( Empire empire, float radius, float density, bool isStatic = false, int segmentCount = 32 )
        {
            Color color = Color.DarkGray;
            var planet = new HomePlanet();

            planet.OwnedBy.Empire = empire;
            planet.LifeStatus.MaximumLifePoints = 500;
            planet.LifeStatus.Refill();

            planet.DrawStrategy = new PhysicsCirlceDrawStrategy() {
                SegmentCount = segmentCount,
                Color = color
            };

            planet.Physics.FixtureDescription = new CircleDescription() {
                Radius = radius,
                Density = density,
                IsStatic = isStatic
            };

            planet.LifeStatus.Destroyed += sender => {
                if( planet.Scene != null )
                {
                    Behave.This( planet )
                        .BlendOut( forSeconds: 10 )                     
                        .Despawn();
                }
            };

            return planet;
        }

        public static Bullet CreateBullet( float density, float activeTime )
        {
            var entity = new Bullet( activeTime ) {
                Name = "Projectile"
            };

            //entity.GravityEmitter.IsEnabled = false;
            entity.DrawStrategy = new PhysicsShapeDrawStrategy();
            entity.Physics.FixtureDescription = new CircleDescription() {
                Radius = 0.1f,
                Density = density
            };

            return entity;
        }

        public static Bullet CreateRocket( float density, float activeTime )
        {
            var entity = new Bullet( activeTime ) {
                Name = "Rocket"
            };

            //entity.GravityEmitter.IsEnabled = false;
            entity.DrawStrategy = new PhysicsShapeDrawStrategy() {
                Color = Color.GreenYellow
            };

            entity.Damaging.Damage = 3;
            entity.Physics.FixtureDescription = new RectangleDescription() {
                Width = 0.1f,
                Height = 0.2f,
                Density = density
            };

            return entity;
        }

        public static Bullet CreateBlackHoleBullet( float bulletDensity, float bulletTime, float holeDensity, float holeTime )
        {
            var entity = new Bullet( bulletTime ) {
                Name = "Black Hole Projectile"
            };

            //entity.GravityEmitter.IsEnabled = false;
            entity.DrawStrategy = new PhysicsShapeDrawStrategy() {
                Color = Color.Yellow
            };

            entity.Physics.FixtureDescription = new CircleDescription() {
                Radius = 0.1f,
                Density = bulletDensity
            };

            entity.Removed += ( sender, scene ) => {
                var hole = CreateBlackHole( holeDensity, holeTime );
                hole.Position = sender.Position;

                scene.AddEntity( hole );
            };

            return entity;
        }

        private static BlackHole CreateBlackHole( float density, float blackHoleActiveTime )
        {
            var hole = new BlackHole() {
                Name = "Black Hole"
            };

            hole.DrawStrategy = new PhysicsShapeDrawStrategy() {
                Color = Color.Violet
            };

            hole.Physics.FixtureDescription = new CircleDescription() {
                Radius = 2.0f,
                Density = density,
                CollisionCategory = Category.None
            };

            Behave.This( hole )
                .BlendIn( forSeconds: 0.7f )
                .AndAtTheSameTime()
                .After( blackHoleActiveTime )
                .BlendOut( forSeconds: 1 )
                .Despawn();

            return hole;
        }

        public static Turrent CreateTurrent( FlyEntity parent = null )
        {
            var entity = new Turrent();

            entity.DrawStrategy = new TurrentDrawStrategy();

            if( parent != null )
            {
                entity.AttachTo( parent );
            }

            return entity;
        }

        public static Asteroid CreateAsteroid( float density, float radius )
        {
            int edgeCount = rand.UncheckedRandomRange( 4, 10 );
            return CreateAsteroid( density, radius, edgeCount, Color.SeaGreen );
        }

        public static Asteroid CreateAsteroid( float density, float radius, int edgeCount, Color color )
        {
            var entity = new Asteroid();
            
            entity.Physics.FixtureDescription = new BreakableCircleDescription() {
                Density = density,
                Radius = radius,
                EdgeCount = edgeCount
            };

            entity.Destroyable.LifePoints = entity.Destroyable.MaximumLifePoints = 3 + edgeCount / 2;
            entity.DrawStrategy = new MultiPhysicsShapeDrawStrategy() {
                Color = color
            };

            return entity;
        }

        public static PowerUp CreateMineral( int amount = 1 )
        {
            var entity = new PowerUp() {
                Name = "Mineral"
            };

            entity.Pickupable.Item = new Items.Mineral() {
                Count = amount
            };

            entity.Physics.FixtureDescription = new TriangleDescription() {
                Density = 0.5f,
                SideSize = 0.5f
            };

            entity.DrawStrategy = new PhysicsShapeDrawStrategy() {
                Color = Color.Yellow
            };

            return entity;
        }

        public static Ship CreateRectangularShip( float width, float height, float density, Empire empire )
        {
            var entity = new Ship();

            entity.LifeStatus.MaximumLifePoints = 100;
            entity.LifeStatus.Refill();

            entity.OwnedBy.Empire = empire;

            entity.DrawStrategy = new PhysicsShapeDrawStrategy();
            entity.Physics.FixtureDescription = new RectangleDescription() {
                Width = width,
                Height = height,
                Density = density,
                IsStatic = false
            };

            return entity;
        }

        public static TractorBeam CreateTractorBeam( IPhysicsEntity from, IPhysicsEntity to )
        {
            if( Atom.Math.Vector2.DistanceSquared( from.Position, to.Position ) >= 700.0f )
            {
                return null;
            }

            var beam = new TractorBeam( from, to );
            beam.DrawStrategy = new TractorBeamDrawStrategy();

            return beam;
        }

        private static readonly RandMT rand = new RandMT();
    }
}
