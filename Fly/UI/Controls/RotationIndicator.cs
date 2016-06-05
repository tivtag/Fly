// <copyright file="RotationIndicator.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.UI.RotationIndicator class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.UI
{
    using System;
    using Atom.Math;
    using Atom.Xna;
    using Fly;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Visualizes the rotation and speed fo the player ship.
    /// </summary>
    public sealed class RotationIndicator : PlayerUIElement
    {
        public bool IsLarge
        {
            get;
            set;
        }

        protected override void OnDraw( IFlyDrawContext drawContext )
        {
            if( this.IsLarge )
            {
                const float LargeIndicatorSize = 50;
                this.DrawRotationIndicator( drawContext, drawContext.ViewWidth/2 - LargeIndicatorSize, drawContext.ViewHeight/2 - LargeIndicatorSize, LargeIndicatorSize );
            }
            else
            {
                this.DrawRotationIndicator( drawContext );
            }
        }

        private void DrawRotationIndicator( IFlyDrawContext drawContext, float StartX = 5, float StartY = 5, float Size = 20 )
        {
            var physics = this.Player.Physics;

            var velocity = physics.LinearVelocity;
            int speed = (int)velocity.Length;

            float rotation;

            if( speed == 0 )
            {
                rotation = -physics.Rotation;
            }
            else
            {
                var velDir = velocity.Direction;
                rotation = (float)Math.Atan2( -velDir.Y, velDir.X );
            }

            // Upper left:
            float sizeRatio = Size * Ratio( rotation, -Constants.Pi + Constants.PiOver4 );
            drawContext.Batch.DrawRect( new RectangleF( StartX + Size - sizeRatio, StartY + Size - sizeRatio, sizeRatio, sizeRatio ), Xna.Color.LightGreen );

            // Upper right:
            sizeRatio = Size * Ratio( rotation, -Constants.PiOver4 );
            drawContext.Batch.DrawRect( new RectangleF( StartX + Size, StartY + Size - sizeRatio, sizeRatio, sizeRatio ), Xna.Color.LightGreen );

            // Lower left:
            sizeRatio = Size * Ratio( rotation, Constants.Pi - Constants.PiOver4 );
            drawContext.Batch.DrawRect( new RectangleF( StartX + Size - sizeRatio, StartY + Size, sizeRatio, sizeRatio ), Xna.Color.LightGreen );

            // Lower right:
            sizeRatio = Size * Ratio( rotation, Constants.PiOver4 );
            drawContext.Batch.DrawRect( new RectangleF( StartX + Size, StartY + Size, sizeRatio, sizeRatio ), Xna.Color.LightGreen );
                        
            var center = new Vector2( StartX+Size, StartY+Size );
            var left = new Vector2( StartX+2*Size, StartY+Size );
            left.Rotate( center, -physics.Rotation );
            
            drawContext.Batch.DrawRect( new RectangleF( left, new Vector2( Size/4, Size/4 ) ), Xna.Color.Red.WithAlpha( 125 ), 1.0f );
        }

        private static float Ratio( float angleA, float angleB )
        {
            float x = Math.Max( angleA, angleB ) - Math.Min( angleA, angleB );
            return ((float)Math.Cos( x ) + 1) / 2.0f;
        }
    }
}
