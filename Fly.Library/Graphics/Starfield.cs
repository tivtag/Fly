// <copyright file="Starfield.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Starfield class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The starfield that renders behind the game, including a parallax effect.
    /// </summary>
    public sealed class Starfield : IDisposable
    {
        /// <summary>
        /// The number of stars in the starfield.
        /// </summary>
        private const int numberOfStars = 25;

        /// <summary>
        /// The number of layers in the starfield.
        /// </summary>
        private const int numberOfLayers = 8;

        /// <summary>
        /// The colors for each layer of stars.
        /// </summary>
        private static readonly Color[] LayerColors = new Color[numberOfLayers] { 
            new Color(255, 255, 255, 255), 
            new Color(255, 255, 255, 216), 
            new Color(255, 255, 255, 192), 
            new Color(255, 255, 255, 160), 
            new Color(255, 255, 255, 128), 
            new Color(255, 205, 205, 96), 
            new Color(255, 155, 155, 64), 
            new Color(255, 55, 55, 32) 
        };

        /// <summary>
        /// The movement factor for each layer of stars, used in the parallax effect.
        /// </summary>
        private static readonly float[] MovementFactors = new float[numberOfLayers] {
            0.9f, 0.8f, 0.7f, 0.6f, 0.5f, 0.4f, 0.3f, 0.2f
        };

        /// <summary>
        /// The maximum amount of movement allowed per update.
        /// </summary>
        /// <remarks>
        /// Any per-update movement values that exceed this will trigger a 
        /// starfield reset.
        /// </remarks>
        private const float MaximumMovementPerUpdate = 128f;

        /// <summary>
        /// The background color of the starfield.
        /// </summary>
        private static readonly Color BackgroundColor = new Color( 0, 0, 32 );

        /// <summary>
        /// The size of each star, in pixels.
        /// </summary>
        private const int StarSize = 2;

        public Starfield( IGraphicsDeviceService deviceService )
        {
            // assign the parameters
            this.deviceService = deviceService;

            // initialize the stars
            stars = new Vector2[numberOfStars];
        }

        /// <summary>
        /// Load graphics data from the system.
        /// </summary>
        public void LoadContent()
        {
            this.device = this.deviceService.GraphicsDevice;

            // create the star texture
            starTexture = new Texture2D( device, 1, 1, false, SurfaceFormat.Color );
            starTexture.SetData<Color>( new Color[] { Color.White } );

            // create the SpriteBatch object
            spriteBatch = new SpriteBatch( device );

            this.Reset( Vector2.Zero );
        }

        /// <summary>
        /// Release graphics data.
        /// </summary>
        public void UnloadContent()
        {
            if( starTexture != null )
            {
                starTexture.Dispose();
                starTexture = null;
            }

            if( spriteBatch != null )
            {
                spriteBatch.Dispose();
                spriteBatch = null;
            }
        }

        /// <summary>
        /// Reset the stars and the parallax effect.
        /// </summary>
        /// <param name="position">The new origin point for the parallax effect.</param>
        public void Reset( Vector2 position )
        {
            // recreate the stars
            int viewportWidth = device.Viewport.Width;
            int viewportHeight = device.Viewport.Height;
            for( int i = 0; i < stars.Length; ++i )
            {
                stars[i] = new Vector2( random.Next( 0, viewportWidth ), random.Next( 0, viewportHeight ) );
            }

            // reset the position
            this.lastPosition = this.position = position;
        }

        /// <summary>
        /// Update and draw the starfield.
        /// </summary>
        /// <remarks>
        /// This function updates and draws the starfield, 
        /// so that the per-star loop is only run once per frame.
        /// </remarks>
        /// <param name="position">The new position for the parallax effect.</param>
        public void Draw( Vector2 position )
        {
            // update the current position
            this.lastPosition = this.position;
            this.position = position;

            // determine the movement vector of the stars
            // -- for the purposes of the parallax effect, 
            //    this is the opposite direction as the position movement.
            Vector2 movement = -1.0f * (position - lastPosition);

            // create a rectangle representing the screen dimensions of the starfield
            Rectangle starfieldRectangle = new Rectangle( 0, 0, device.Viewport.Width, device.Viewport.Height );

            // if we've moved too far, then reset, as the stars will be moving too fast
            if( movement.Length() > MaximumMovementPerUpdate )
            {
                Reset( position );
                return;
            }

            // draw all of the stars
            spriteBatch.Begin( SpriteSortMode.Immediate, BlendState.NonPremultiplied );

            for( int starIndex = 0; starIndex < stars.Length; ++starIndex )
            {
                // move the star based on the depth
                int depth = starIndex % MovementFactors.Length;
                Vector2 star = stars[starIndex];

                star += movement * MovementFactors[depth];

                // wrap the stars around
                if( star.X < starfieldRectangle.X )
                {
                    star.X = starfieldRectangle.X + starfieldRectangle.Width;
                    star.Y = starfieldRectangle.Y + random.Next( starfieldRectangle.Height );
                }

                if( star.X > (starfieldRectangle.X + starfieldRectangle.Width) )
                {
                    star.X = starfieldRectangle.X;
                    star.Y = starfieldRectangle.Y + random.Next( starfieldRectangle.Height );
                }

                if( star.Y < starfieldRectangle.Y )
                {
                    star.X = starfieldRectangle.X + random.Next( starfieldRectangle.Width );
                    star.Y = starfieldRectangle.Y + starfieldRectangle.Height;
                }

                if( star.Y > (starfieldRectangle.Y + device.Viewport.Height) )
                {
                    star.X = starfieldRectangle.X + random.Next( starfieldRectangle.Width );
                    star.Y = starfieldRectangle.Y;
                }

                // draw the star
                spriteBatch.Draw(
                    starTexture,
                    new Rectangle( (int)star.X, (int)star.Y, StarSize, StarSize ),
                    null,
                    LayerColors[depth]
                );

                stars[starIndex] = star;
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Disposes the Starfield object.
        /// </summary>
        public void Dispose()
        {
            if( starTexture != null )
            {
                starTexture.Dispose();
                starTexture = null;
            }

            if( spriteBatch != null )
            {
                spriteBatch.Dispose();
                spriteBatch = null;
            }
        }

        /// <summary>
        /// The last position, used for the parallax effect.
        /// </summary>
        private Vector2 lastPosition;

        /// <summary>
        /// The current position, used for the parallax effect.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The stars in the starfield.
        /// </summary>
        private Vector2[] stars;

        /// <summary>
        /// The graphics device used to render the starfield.
        /// </summary>
        private GraphicsDevice device;

        /// <summary>
        /// The SpriteBatch used to render the starfield.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// The texture used for each star, typically one white pixel.
        /// </summary>
        private Texture2D starTexture;

        private readonly Random random = new Random();
        private readonly IGraphicsDeviceService deviceService;
    }
}

