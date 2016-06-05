// <copyright file="IBoundCalculator.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.IBoundCalculator interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Atom.Components;

    /// <summary>
    /// Adds a mechanism for calculating the bounding area (collision area) to an entity.
    /// </summary>
    public interface IBoundCalculator : IComponent
    {
        Atom.Math.RectangleF GetBounds();
    }
}
