// <copyright file="ShapeRenderer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.ShapeRenderer class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics
{
    using System;
    using System.Diagnostics;
    using Atom.Xna;
    using Atom.Xna.Effects;
    using FarseerPhysics;
    using FarseerPhysics.Collision.Shapes;
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Implements an <see cref="IShapeRenderer"/> that queues up all drawing calls to output them in a single drawing call to the rendering target.
    /// </summary>
    public sealed class ShapeRenderer : IShapeRenderer
    {
        public void LoadContent( GraphicsDevice device, IEffectLoader effectLoader )
        {
            this.device = device;

            this.effect = effectLoader.Load( "Shapes" );
            this.effectPass = effect.Techniques[0].Passes[0];
        }

        public void DrawSolidCircle( CircleShape circle, Transform transform, Color color, int segments = 32 )
        {
            Vector2 center = MathUtils.Multiply( ref transform, circle.Position );
            float radius = circle.Radius;
            Vector2 axis = transform.R.Col1;

            this.DrawSolidCircle( center, radius, axis, color, segments );
        }

        public void DrawShape( Fixture fixture, Transform transform, Color color )
        {
            switch( fixture.ShapeType )
            {
                case ShapeType.Circle:
                    {
                        CircleShape circle = (CircleShape)fixture.Shape;

                        Vector2 center = MathUtils.Multiply( ref transform, circle.Position );
                        float radius = circle.Radius;
                        Vector2 axis = transform.R.Col1;

                        this.DrawSolidCircle( center, radius, axis, color );
                    }
                    break;

                case ShapeType.Polygon:
                    {
                        PolygonShape poly = (PolygonShape)fixture.Shape;
                        int vertexCount = poly.Vertices.Count;

                        Debug.Assert( vertexCount <= Settings.MaxPolygonVertices );
                        Vector2[] vertices = new Vector2[Settings.MaxPolygonVertices];

                        for( int i = 0; i < vertexCount; ++i )
                        {
                            vertices[i] = MathUtils.Multiply( ref transform, poly.Vertices[i] );
                        }

                        this.DrawSolidPolygon( vertices, vertexCount, color );
                    }
                    break;

                case ShapeType.Edge:
                    {
                        EdgeShape edge = (EdgeShape)fixture.Shape;
                        Vector2 v1 = MathUtils.Multiply( ref transform, edge.Vertex1 );
                        Vector2 v2 = MathUtils.Multiply( ref transform, edge.Vertex2 );
                        this.DrawSegment( v1, v2, color );
                    }
                    break;

                case ShapeType.Loop:
                    {
                        LoopShape loop = (LoopShape)fixture.Shape;
                        int count = loop.Vertices.Count;

                        Vector2 v1 = MathUtils.Multiply( ref transform, loop.Vertices[count - 1] );
                        for( int i = 0; i < count; ++i )
                        {
                            Vector2 v2 = MathUtils.Multiply( ref transform, loop.Vertices[i] );
                            DrawSegment( v1, v2, color );
                            v1 = v2;
                        }
                    }
                    break;
            }
        }

        public void DrawPolygon( Vector2[] vertices, int count, Color color )
        {
            for( int i = 0; i < count - 1; ++i )
            {
                int lineCountTwice = this.lineCount * 2;
                this.vertsLines[lineCountTwice].Position = new Vector3( vertices[i], 0.0f );
                this.vertsLines[lineCountTwice].Color = color;
                this.vertsLines[lineCountTwice + 1].Position = new Vector3( vertices[i + 1], 0.0f );
                this.vertsLines[lineCountTwice + 1].Color = color;
                ++this.lineCount;
            }

            this.vertsLines[this.lineCount * 2].Position = new Vector3( vertices[count - 1], 0.0f );
            this.vertsLines[this.lineCount * 2].Color = color;
            this.vertsLines[this.lineCount * 2 + 1].Position = new Vector3( vertices[0], 0.0f );
            this.vertsLines[this.lineCount * 2 + 1].Color = color;
            ++this.lineCount;
        }

        public void DrawCircle( Vector2 center, float radius, Color color, int segments = 32 )
        {
            double increment = Math.PI * 2.0 / segments;
            double theta = 0.0;

            for( int i = 0; i < segments; ++i )
            {
                Vector2 v1 = center + radius * new Vector2( (float)Math.Cos( theta ), (float)Math.Sin( theta ) );
                Vector2 v2 = center +
                             radius *
                             new Vector2( (float)Math.Cos( theta + increment ), (float)Math.Sin( theta + increment ) );

                this.vertsLines[this.lineCount * 2].Position = new Vector3( v1, 0.0f );
                this.vertsLines[this.lineCount * 2].Color = color;
                this.vertsLines[this.lineCount * 2 + 1].Position = new Vector3( v2, 0.0f );
                this.vertsLines[this.lineCount * 2 + 1].Color = color;
                ++this.lineCount;

                theta += increment;
            }
        }

        public void DrawSolidSegment( Vector2 start, Vector2 end, Color color, float width )
        {
            float halfWidth = width / 2.0f;
            var localEnd = (end - start).ToAtom();
            var perp = localEnd.Perpendicular.Direction;

            vertexBuffer4[0] = (start.ToAtom() - (perp * halfWidth)).ToXna();
            vertexBuffer4[1] = (end.ToAtom() - (perp * halfWidth)).ToXna();
            vertexBuffer4[2] = (end.ToAtom() + (perp * halfWidth)).ToXna();
            vertexBuffer4[3] = (start.ToAtom() + (perp * halfWidth)).ToXna();

            this.DrawSolidPolygon( vertexBuffer4, 4, color );
        }

        public void DrawSolidPolygon( Vector2[] vertices, int count, Color color, bool outline = true )
        {
            if( count == 2 )
            {
                this.DrawPolygon( vertices, count, color );
                return;
            }

            Color colorFill = new Color( color.R, color.G, color.B, color.A / 2 );

            for( int i = 1; i < count - 1; ++i )
            {
                this.vertsFill[this.fillCount * 3].Position = new Vector3( vertices[0], 0.0f );
                this.vertsFill[this.fillCount * 3].Color = colorFill;

                this.vertsFill[this.fillCount * 3 + 1].Position = new Vector3( vertices[i], 0.0f );
                this.vertsFill[this.fillCount * 3 + 1].Color = colorFill;

                this.vertsFill[this.fillCount * 3 + 2].Position = new Vector3( vertices[i + 1], 0.0f );
                this.vertsFill[this.fillCount * 3 + 2].Color = colorFill;

                this.fillCount++;
            }

            if( outline )
            {
                this.DrawPolygon( vertices, count, color );
            }
        }

        public void DrawSolidCircle( Vector2 center, float radius, Vector2 axis, Color color, int segments = 32 )
        {
            double increment = Math.PI * 2.0 / segments;
            double theta = 0.0;

            Color colorFill = new Color( color.R, color.G, color.B, color.A / 2 );

            Vector2 v0 = center + radius * new Vector2( (float)Math.Cos( theta ), (float)Math.Sin( theta ) );
            theta += increment;

            for( int i = 1; i < segments - 1; i++ )
            {
                Vector2 v1 = center + radius * new Vector2( (float)Math.Cos( theta ), (float)Math.Sin( theta ) );
                Vector2 v2 = center +
                             radius *
                             new Vector2( (float)Math.Cos( theta + increment ), (float)Math.Sin( theta + increment ) );

                this.vertsFill[this.fillCount * 3].Position = new Vector3( v0, 0.0f );
                this.vertsFill[this.fillCount * 3].Color = colorFill;
                this.vertsFill[this.fillCount * 3 + 1].Position = new Vector3( v1, 0.0f );
                this.vertsFill[this.fillCount * 3 + 1].Color = colorFill;
                this.vertsFill[this.fillCount * 3 + 2].Position = new Vector3( v2, 0.0f );
                this.vertsFill[this.fillCount * 3 + 2].Color = colorFill;

                ++this.fillCount;
                theta += increment;
            }

            this.DrawCircle( center, radius, color, segments );
            this.DrawSegment( center, center + axis * radius, color );
        }

        public void DrawSolidCircleWithoutRotationIndicator( Vector2 center, float radius, Vector2 axis, Color color, int segments = 32 )
        {
            double increment = Math.PI * 2.0 / segments;
            double theta = 0.0;

            Color colorFill = new Color( color.R, color.G, color.B, color.A / 2 );

            Vector2 v0 = center + radius * new Vector2( (float)Math.Cos( theta ), (float)Math.Sin( theta ) );
            theta += increment;

            for( int i = 1; i < segments - 1; i++ )
            {
                Vector2 v1 = center + radius * new Vector2( (float)Math.Cos( theta ), (float)Math.Sin( theta ) );
                Vector2 v2 = center +
                             radius *
                             new Vector2( (float)Math.Cos( theta + increment ), (float)Math.Sin( theta + increment ) );

                this.vertsFill[this.fillCount * 3].Position = new Vector3( v0, 0.0f );
                this.vertsFill[this.fillCount * 3].Color = colorFill;
                this.vertsFill[this.fillCount * 3 + 1].Position = new Vector3( v1, 0.0f );
                this.vertsFill[this.fillCount * 3 + 1].Color = colorFill;
                this.vertsFill[this.fillCount * 3 + 2].Position = new Vector3( v2, 0.0f );
                this.vertsFill[this.fillCount * 3 + 2].Color = colorFill;

                ++this.fillCount;
                theta += increment;
            }

            this.DrawCircle( center, radius, color );
        }

        public void DrawSegment( Vector2 start, Vector2 end, Color color )
        {
            this.vertsLines[this.lineCount * 2].Color = color;
            this.vertsLines[this.lineCount * 2].Position = new Vector3( start, 0.0f );
            this.vertsLines[this.lineCount * 2 + 1].Position = new Vector3( end, 0.0f );
            this.vertsLines[this.lineCount * 2 + 1].Color = color;
            ++this.lineCount;
        }

        public void DrawTransform( ref FarseerPhysics.Common.Transform transform )
        {
            const float AxisScale = 0.4f;
            Vector2 p1 = transform.Position;

            Vector2 p2 = p1 + AxisScale * transform.R.Col1;
            this.DrawSegment( p1, p2, Color.Red );

            p2 = p1 + AxisScale * transform.R.Col2;
            this.DrawSegment( p1, p2, Color.Green );
        }

        public void DrawPoint( Vector2 position, float size, Color color )
        {
            Vector2[] verts = new Vector2[4];
            float hs = size / 2.0f;
            verts[0] = position + new Vector2( -hs, -hs );
            verts[1] = position + new Vector2( hs, -hs );
            verts[2] = position + new Vector2( hs, hs );
            verts[3] = position + new Vector2( -hs, hs );

            this.DrawSolidPolygon( verts, 4, color, true );
        }

        public void RenderOutput( ref Matrix projection, ref Matrix view )
        {
            // set the effects projection matrix
            effect.Parameters["Projection"].SetValue( projection );
            effect.Parameters["View"].SetValue( view );

            // we should have only 1 technique and 1 pass
            effectPass.Apply();
            {
                device.RasterizerState = RasterizerState.CullNone;

                if( fillCount > 0 )
                {
                    device.DrawUserPrimitives( PrimitiveType.TriangleList, vertsFill, 0, fillCount );
                }

                if( lineCount > 0 )
                {
                    device.DrawUserPrimitives( PrimitiveType.LineList, vertsLines, 0, lineCount );
                }
            }

            this.lineCount = 0;
            this.fillCount = 0;
        }

        private int lineCount;
        private int fillCount;
        private Effect effect;
        private GraphicsDevice device;
        private EffectPass effectPass;

        private readonly Vector2[] vertexBuffer4 = new Vector2[4];

        private readonly VertexPositionColor[] vertsLines = new VertexPositionColor[1000000];
        private readonly VertexPositionColor[] vertsFill = new VertexPositionColor[1000000];
    }
}
