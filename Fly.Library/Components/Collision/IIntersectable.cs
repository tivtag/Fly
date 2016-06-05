// <copyright file="IIntersectable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.IIntersectable interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    /// <summary>
    /// Adds a mechanism for checking for intersection to entity.
    /// </summary>
    public interface IIntersectable : IFlyComponent
    {
        bool IntersectsAt( Atom.Math.Vector2 point );
    }
}
