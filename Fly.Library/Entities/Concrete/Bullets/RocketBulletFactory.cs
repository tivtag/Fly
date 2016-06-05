// <copyright file="RocketBulletFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.Concrete.RocketBulletFactory class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities.Concrete
{
    using Atom.Math;

    /// <summary>
    /// Builds new Rocket-type <see cref="Bullet"/> instances.
    /// Rockets deal more damage, have a higher density and move slower.
    /// </summary>
    public class RocketBulletFactory : BulletFactory
    {
        public RocketBulletFactory()
        {
            this.BulletDensity = 100.0f;
            this.BaseBulletSpeed = 350.0f;
        }

        public override Bullet CreateBullet( Turrent turrent )
        {
            var bullet = EntityFactory.CreateRocket( this.BulletDensity, this.BulletTime );
            bullet.Rotation = turrent.Rotation - Constants.PiOver2;

            return bullet;
        }
    }
}
