// <copyright file="TierAShipDrawStrategy.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.Strategies.Ships.TierAShipDrawStrategy class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics.Strategies.Ships
{
    using Atom.Math;
    using Atom.Xna;

    /// <summary>
    /// Implements an <see cref="IEntityDrawStrategy"/> that draws a Tier-A (player) ship.
    /// </summary>
    public sealed class TierAShipDrawStrategy : PhysicsShapeDrawStrategy
    {
        public override void Draw( IFlyDrawContext drawContext )
        {
            base.Draw( drawContext );

            Vector2 normal = Vector2.FromAngle( this.Entity.Rotation );
            Vector2 perp = normal.Perpendicular;

            // Turpine A:
            Vector2 start = this.Entity.Position - normal * 0.25f + (perp * 0.45f);
            Vector2 end = start - (normal * 0.1f);

            drawContext.ShapeRenderer.DrawSolidSegment( start.ToXna(), end.ToXna(), this.FinalColor, 0.1f );
            
            // Turpine B:
            start = this.Entity.Position - normal * 0.25f - (perp * 0.45f);
            end = start - (normal * 0.1f);

            drawContext.ShapeRenderer.DrawSolidSegment( start.ToXna(), end.ToXna(), this.FinalColor, 0.1f );

            //// // Cockpit A:
            //// start = this.Entity.Position + (normal * 0.20f) - (perp * 0.15f);
            //// end = start + (perp * 0.30f);
            //// drawContext.ShapeRenderer.DrawSolidSegment( start.ToXna(), end.ToXna(), this.FinalColor, 0.1f );
        }
    }
}
