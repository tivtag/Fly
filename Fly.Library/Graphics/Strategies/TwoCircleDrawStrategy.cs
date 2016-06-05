// <copyright file="TwoCircleDrawStrategy.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.Strategies.TwoCircleDrawStrategy class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics.Strategies
{
    using Atom.Xna;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Implements an <seealso cref="IEntityDrawStrategy"/> that draws a circle and an	accompanying smaller circle positioned at the center of the entity.
    /// </summary>
    public sealed class TwoCircleDrawStrategy : EntityDrawStrategy
    {
        public float Radius 
        {
            get; 
            set; 
        }

        public override void Draw( IFlyDrawContext drawContext )
        {
            Xna.Vector2 center = this.Entity.Position.ToXna();
            IShapeRenderer renderer = drawContext.ShapeRenderer;

            renderer.DrawSolidCircleWithoutRotationIndicator( center, this.Radius,        Xna.Vector2.One, this.FinalColor );
            renderer.DrawSolidCircleWithoutRotationIndicator( center, this.Radius * 0.2f, Xna.Vector2.One, this.FinalColor );
        }
    }
}
