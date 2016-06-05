// <copyright file="IFlyDrawable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.IFlyDrawable interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using Atom;

    /// <summary>
    /// Provides the mechanism of drawing an object in the Fly world.
    /// </summary>
    public interface IFlyDrawable : IDrawable
    {
        void Draw( IFlyDrawContext drawContext );
    }
}
