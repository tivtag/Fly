// <copyright file="BulletFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.Concrete.BulletFactory class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities.Concrete
{
    /// <summary>
    /// Implements a mechanism for creating new <see cref="Bullet"/> instance that are ready to be launced.
    /// </summary>
    public class BulletFactory : IBulletFactory
    {
        public float BaseBulletSpeed { get; set; }
        public float BulletDensity { get; set; }
        public float BulletTime { get; set; }

        public BulletFactory()
        {
            this.BulletDensity = 10.0f;
            this.BulletTime = 10.0f;
            this.BaseBulletSpeed = 650.0f;
        }

        public virtual Bullet CreateBullet( Turrent turrent )
        {
            return EntityFactory.CreateBullet( this.BulletDensity, this.BulletTime );
        }
    }
}
