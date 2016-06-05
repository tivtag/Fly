// <copyright file="MixInColorTint.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.Tinting.MixInColorTint class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics.Tinting
{
    using Atom.Math;

    /// <summary>
    /// Defines a <see cref="TimedColorTint"/> that blends
    /// the mixes the input color into a target color.
    /// </summary>
    public class MixInColorTint : TimedColorTint
    {
        public Vector4 TargetColor
        {
            get;
            set;
        }

        public override Vector4 Apply( Vector4 color )
        {
            if( this.HasReachedFullEffect )
            {
                return this.TargetColor;
            }
            else
            {
                return (color * (this.Factor)) + (this.TargetColor * (1.0f - this.Factor));
            }
        }
    }
}
