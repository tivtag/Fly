// <copyright file="BlackHoleBulletFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.Concrete.BlackHoleBulletFactory class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities.Concrete
{
    /// <summary>
    /// Builds new Black Hole-type <see cref="Bullet"/> instances.
    /// Black Holes emit a very strong gravitational effect that attracts all gravity-receiving entities.
    /// </summary>
    public class BlackHoleBulletFactory : BulletFactory
    {
        public BlackHoleBulletFactory()
        {
            this.BulletDensity = 60.0f;
            this.BaseBulletSpeed = 450.0f;
        }

        public override Bullet CreateBullet( Turrent turrent )
        {
            return EntityFactory.CreateBlackHoleBullet( this.BulletDensity, 2.0f, 90000.0f, 15.0f );
        }
    }
}
