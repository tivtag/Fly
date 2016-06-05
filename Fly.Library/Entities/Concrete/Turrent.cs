// <copyright file="Turrent.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.Concrete.Turrent class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities.Concrete
{
    using Atom.Math;
    using Fly.Components;

    /// <summary>
    /// Represents a Turrent that can launch <see cref="Bullet"/>s.
    /// </summary>
    public sealed class Turrent : FlyEntity
    {
        public float Radius 
        {
            get;
            set;
        }

        public float BulletSpawnOffset 
        {
            get;
            set;
        }

        public float BulletSpawnDistance
        {
            get
            {
                return this.Radius + this.BulletSpawnOffset;
            }
        }

        public bool IsRotationLockedToParent
        {
            get
            {
                return this.Transform.InheritsRotation;
            }

            set
            {
                this.Transform.InheritsRotation = value;
            }
        }

        public IBulletFactory BulletFactory 
        { 
            get; 
            set;
        }

        public Turrent()
        {
            this.Radius = 0.2f;
            this.BulletSpawnOffset = 0.3f;
            this.IsRotationLockedToParent = true;

            this.BulletFactory = new BulletFactory();
        }
        
        public void Shoot()
        {
            ShootTwoBullets();
        }

        private void ShootTwoBullets()
        {
            Bullet bulletA = this.BulletFactory.CreateBullet( this );
            this.SetupBullet( bulletA, -0.70f );

            Bullet bulletB = this.BulletFactory.CreateBullet( this );
            this.SetupBullet( bulletB, 0.70f );
        }

        private void SetupBullet( Bullet bullet, float offset )
        {
            Vector2 normal = Vector2.FromAngle( this.Rotation );
            Vector2 perp = normal.Perpendicular;

            bullet.Position = this.Position + normal * this.BulletSpawnDistance + perp * offset;
            
            this.Scene.AddEntity( bullet );
            this.PushBullet( bullet, normal );
        }

        private void PushBullet( Bullet bullet, Vector2 normal )
        {
            float speed = this.BulletFactory.BaseBulletSpeed;

            if( this.Parent != null )
            {
                var physicable = this.Parent.Components.Find<IPhysicable>();
                Vector2 velocity = physicable.LinearVelocity;

                bullet.Physics.LinearVelocity = velocity;
                speed += velocity.Length * 3;
            }

            bullet.Physics.ApplyForce( normal * (speed * bullet.Physics.Mass) );
        }
    }
}
