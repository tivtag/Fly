// <copyright file="IBulletFactory.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.Concrete.IBulletFactory interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities.Concrete
{
    /// <summary>
    /// Provides a mechanism for creatnig new <see cref="Bullet"/> instance that are ready to be launced.
    /// </summary>
    public interface IBulletFactory
    {
        float BaseBulletSpeed { get; }

        Bullet CreateBullet( Turrent turrent );
    }
}
