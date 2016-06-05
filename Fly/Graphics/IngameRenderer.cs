// <copyright file="IngameRenderer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.IngameRenderer class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics
{
    using Atom.Xna.UI;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Responsible for rendering all that is seen.
    /// This GameWorld with all it's entities, shapes, debug objects and the User Interface ontop of it. 
    /// </summary>
    public sealed class IngameRenderer
    {
        public FlyScene Scene
        {
            get { return scene; }
            set { scene = value; }
        }

        public UserInterface UserInterface
        {
            get { return userInterface; }
            set { userInterface = value; }
        }
        
        public IngameRenderer( IGraphicsDeviceService deviceService, IRenderPipeline renderPipeline )
        {
            this.deviceService = deviceService;
            this.starfield = new Starfield( this.deviceService );

            this.renderPipeline = renderPipeline;
        }

        public void Load()
        {            
            this.starfield.LoadContent();
        }

        public void Draw( IFlyDrawContext drawContext )
        {
            this.renderPipeline.RenderAction = this.DrawUnprocessed;
            this.renderPipeline.Draw( drawContext );
        }

        private void DrawUnprocessed( IFlyDrawContext drawContext )
        {
            this.DrawBackground( drawContext );
            this.DrawContent( drawContext );
            this.OutputShapes( drawContext );
            this.DrawOverlays( drawContext );
        }

        private void DrawBackground( IFlyDrawContext drawContext )
        {
            var camera = drawContext.Camera;
            starfield.Draw( new Xna.Vector2( camera.Scroll.X, -camera.Scroll.Y ) );
        }

        private void DrawContent( IFlyDrawContext drawContext )
        {
            this.scene.Draw( drawContext );
        }

        private void OutputShapes( IFlyDrawContext drawContext )
        {
            var camera = drawContext.Camera;
            drawContext.ShapeRenderer.RenderOutput( ref camera.Projection, ref camera.View );
        }

        private void DrawOverlays( IFlyDrawContext drawContext )
        {
            if( this.userInterface != null )
            {
                this.userInterface.Draw( drawContext );
            }
        }

        private FlyScene scene;
        private UserInterface userInterface;

        private readonly Starfield starfield;
        private readonly IGraphicsDeviceService deviceService;
        private readonly IRenderPipeline renderPipeline;
    }
}
