// <copyright file="GameGraphics.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.GameGraphics class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics
{
    using Atom.Xna;
    using Atom.Xna.Batches;
    using Atom.Xna.Effects;
    using Atom.Xna.Fonts;
    using Fly.UI;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Responsible for configuration and setup of the XNA Game graphics framework.
    /// </summary>
    public sealed class GameGraphics
    {
        public GraphicsDevice Device
        {
            get
            {
                return this.device;
            }
        }

        public Xna.GameWindow Window
        {
            get
            {
                return this.window;
            }
        }

        public IRenderTarget2DFactory RenderTargetFactory
        {
            get
            {
                return this.renderTargetFactory;
            }
        }

        public IFlyDrawContext DrawContext
        {
            get
            {
                return this.drawContext;
            }
        }

        public IEffectLoader EffectLoader
        {
            get
            {
                return this.effectLoader;
            }
        }

        public IRenderPipeline Pipeline
        {
            get
            {
                return this.pipeline;
            }
        }

        public IGraphicsDeviceService DeviceService
        {
            get
            {
                return this.graphics;
            }
        }

        public IViewSizeService ViewSizeService
        {
            get
            {
                return this.viewSizeService;
            }
        }

        public GameGraphics( FlyGame game )
        {
            this.game = game;
            this.graphics = new Xna.GraphicsDeviceManager( game );
            this.graphics.IsFullScreen = false;
            this.graphics.PreparingDeviceSettings += this.OnPreparingDeviceSettings;
            this.graphics.GraphicsProfile = GraphicsProfile.HiDef;
                        
            this.fontLoader = new FontLoader( this.game.Services ) { RootDirectory = "Content/Fonts/" };
        }

        public void Load()
        {
            this.device = this.game.GraphicsDevice;
            this.window = this.game.Window;

            this.effectLoader        = Atom.Xna.Effects.EffectLoader.Create( this.game.Services );
            this.viewSizeService     = new GameWindowViewSizeService( this.game.Window );
            this.renderTargetFactory = new RenderTarget2DFactory( this.viewSizeService.ViewSize, this.graphics, 16 );

            this.drawContext = this.CreateDrawContext();
            this.pipeline = new BloomRenderPipeline( this.effectLoader, this.renderTargetFactory, this.graphics );
            this.pipeline.Load();

            UIFonts.Load( this.fontLoader );
        }

        private IFlyDrawContext CreateDrawContext()
        {
            return new FlyDrawContext( this.device, this.viewSizeService, this.effectLoader ) {
                Batch = new ComposedSpriteBatch( this.device )
            };
        }

        private void OnPreparingDeviceSettings( object sender, Xna.PreparingDeviceSettingsEventArgs e )
        {
            var pp = e.GraphicsDeviceInformation.PresentationParameters;
            var desktopDisplayMode = e.GraphicsDeviceInformation.Adapter.CurrentDisplayMode;

            pp.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            pp.IsFullScreen = true;

            if( pp.IsFullScreen )
            {
                pp.BackBufferWidth = desktopDisplayMode.Width;
                pp.BackBufferHeight = desktopDisplayMode.Height;
            }
            else
            {
                var form = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle( this.game.Window.Handle );
                int borderX = form.Size.Width - form.ClientSize.Width;
                int borderY = form.Size.Height - form.ClientSize.Height;

                pp.BackBufferWidth = desktopDisplayMode.Width - borderX;
                pp.BackBufferHeight = desktopDisplayMode.Height - borderY;
            }
        }
        
        private IFlyDrawContext drawContext;
        private IEffectLoader effectLoader;
        private IRenderTarget2DFactory renderTargetFactory;
        private IViewSizeService viewSizeService;
        private GraphicsDevice device;
        private Xna.GameWindow window;
        private Xna.GraphicsDeviceManager graphics;

        private IRenderPipeline pipeline;
        private readonly IFontLoader fontLoader;
        private readonly FlyGame game;
    }
}
