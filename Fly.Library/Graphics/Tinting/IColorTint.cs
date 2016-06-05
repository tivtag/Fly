// <copyright file="IColorTint.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.Tinting.IColorTint interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics.Tinting
{
    using Atom;
    using Atom.Math;

    /// <summary>
    /// A color tint is an effect that transform an input color into an output color.
    /// The effect might change over time (<seealso cref="IUpdateable"/>).
    /// </summary>
    public interface IColorTint : IUpdateable
    {
        /// <summary>
        /// Applies this IColorTint to the given color.
        /// </summary>
        /// <param name="color">
        /// The input color.
        /// </param>
        /// <returns>
        /// The output color.
        /// </returns>
        Vector4 Apply( Vector4 color );
    }
}
