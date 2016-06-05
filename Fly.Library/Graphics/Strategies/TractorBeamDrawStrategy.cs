// <copyright file="TractorBeamDrawStrategy.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.Strategies.TractorBeamDrawStrategy class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics.Strategies
{
    using Fly.Entities.Concrete;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Implements an <seealso cref="IEntityDrawStrategy"/> for <see cref="TractorBeam"/>s.
    /// </summary>
    public sealed class TractorBeamDrawStrategy : EntityDrawStrategy<TractorBeam>
    {
        public TractorBeamDrawStrategy()
        {
            this.Color = Color.Yellow;
        }

        public override void Draw( IFlyDrawContext drawContext )
        {
            var joint = this.Entity.Joint;
            drawContext.ShapeRenderer.DrawSolidSegment( joint.WorldAnchorA, joint.WorldAnchorB, this.FinalColor, 0.05f );
        }
    }
}
