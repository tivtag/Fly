// <copyright file="IScaleable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.IScaleable interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using Atom.Math;

    /// <summary>
    /// Represents an object that is scaleable by a 2-dimensional factor.
    /// </summary>
    public interface IScaleable
    {
        void ScaleTo( Vector2 factor );
    }
}
