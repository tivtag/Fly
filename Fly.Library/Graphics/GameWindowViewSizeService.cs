// <copyright file="GameWindowViewSizeService.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.GameWindowViewSizeService class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics
{
    using Atom.Math;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Implements an <see cref="IViewSizeService"/> that derives the view size from the GameWindow's size.
    /// </summary>
    public sealed class GameWindowViewSizeService : IViewSizeService
    {
        public Point2 ViewSize
        {
            get 
            {
                return this.size;
            }
        }

        public int ViewWidth
        {
            get 
            {
                return this.size.X;
            }
        }

        public int ViewHeight
        {
            get
            {
                return this.size.Y;
            }
        }

        public GameWindowViewSizeService( Xna.GameWindow window )
        {
            this.window = window;
            this.window.ClientSizeChanged += this.OnClientSizeChanged;

            this.RefreshCache();
        }
        
        private void OnClientSizeChanged( object sender, System.EventArgs e )
        {
            this.RefreshCache();
        }

        private void RefreshCache()
        {
            Xna.Rectangle bounds = this.window.ClientBounds;
            this.size = new Point2( bounds.Width, bounds.Height );
        }

        private Point2 size;
        private readonly Xna.GameWindow window;
    }
}
