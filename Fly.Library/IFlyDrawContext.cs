// <copyright file="IFlyDrawContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.IFlyDrawContext interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using Atom.Xna;
    using Fly.Graphics;

    /// <summary>
    /// Represents context that is passed to the Draw method of all entities, components, etc.
    /// </summary>
    /// <seealso cref="IFlyDrawable"/>
    public interface IFlyDrawContext : ISpriteDrawContext, IViewSizeService
    {
        IShapeRenderer ShapeRenderer { get; }

        Camera Camera { get; set; }
    }
}
