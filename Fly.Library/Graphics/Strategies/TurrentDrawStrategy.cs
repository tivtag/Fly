// <copyright file="TurrentDrawStrategy.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.Strategies.TurrentDrawStrategy class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics.Strategies
{
    using Atom.Math;
    using Atom.Xna;
    using Fly.Entities.Concrete;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Implements an <seealso cref="IEntityDrawStrategy"/> for <see cref="Turrent"/>s.
    /// </summary>
    public sealed class TurrentDrawStrategy : EntityDrawStrategy<Turrent>
    {
        public override void Draw( IFlyDrawContext drawContext )
        {
            var renderer = drawContext.ShapeRenderer;
            renderer.DrawSolidCircleWithoutRotationIndicator( this.Entity.Parent.Position.ToXna(), this.Entity.Radius, Xna.Vector2.One, this.ApplyTinting( Xna.Color.Black ) );

            var end = this.Entity.Position + Vector2.FromAngle( this.Entity.Rotation ) * (this.Entity.BulletSpawnDistance * 1.1f);
            renderer.DrawSegment( this.Entity.Parent.Position.ToXna(), end.ToXna(), this.ApplyTinting( Xna.Color.White ) );
        }
    }
}
