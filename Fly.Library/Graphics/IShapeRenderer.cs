// <copyright file="IShapeRenderer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.IShapeRenderer interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics
{
    using Atom.Xna.Effects;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using FarseerPhysics.Collision.Shapes;
    using FarseerPhysics.Common;

    /// <summary>
    /// Provides a mechanism for drawing various basic shapes.
    /// In the Fly game world all is made out of these basics shapes.
    /// </summary>
    public interface IShapeRenderer
    {
        void DrawCircle( Vector2 center, float radius, Color color, int segments = 32 );
        void DrawPoint( Vector2 position, float size, Color color );
        void DrawPolygon( Vector2[] vertices, int count, Color color );
        void DrawSegment( Vector2 start, Vector2 end, Color color );
        void DrawShape( FarseerPhysics.Dynamics.Fixture fixture, Transform transform, Color color );

        void DrawSolidSegment( Vector2 start, Vector2 end, Color color, float width );
        void DrawSolidCircle( CircleShape circle, Transform transform, Color color, int segments = 32 );
        void DrawSolidCircle( Vector2 center, float radius, Vector2 axis, Color color, int segments = 32 );
        void DrawSolidCircleWithoutRotationIndicator( Vector2 center, float radius, Vector2 axis, Color color, int segments = 32 );
        void DrawSolidPolygon( Vector2[] vertices, int count, Color color, bool outline = true );
        void DrawTransform( ref FarseerPhysics.Common.Transform transform );

        void LoadContent( GraphicsDevice device, IEffectLoader effectLoader );
        void RenderOutput( ref Matrix projection, ref Matrix view );
    }
}
