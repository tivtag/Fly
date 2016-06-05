// <copyright file="BloomRenderPipeline.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.BloomRenderPipeline class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics
{
    using System;
    using Atom;
    using Atom.Xna;
    using Atom.Xna.Effects;
    using Atom.Xna.Effects.PostProcess;
    using Microsoft.Xna.Framework.Graphics;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Implements an IRenderPipeline that adds a shiny bloom post-processing filter
    /// at the end of the rendering process.
    /// </summary>
    public sealed class BloomRenderPipeline : IRenderPipeline
    {
        public Action<IFlyDrawContext> RenderAction
        {
            get;
            set;
        }

        public BloomRenderPipeline( IEffectLoader effectLoader, IRenderTarget2DFactory renderTargetFactory, IGraphicsDeviceService deviceService )
        {
            this.bloom = new Bloom( effectLoader, renderTargetFactory, deviceService );
            this.bloom.Settings = BloomSettings.Default;

            this.renderTargetFactory = renderTargetFactory;
            this.deviceService = deviceService;
        }

        public void Load()
        {
            this.bloom.LoadContent();

            this.device = this.deviceService.GraphicsDevice;
            this.renderTarget = this.renderTargetFactory.Create();
        }

        public void Draw( IFlyDrawContext drawContext )
        {
            this.device.SetRenderTarget( renderTarget );
            this.device.BlendState = BlendState.NonPremultiplied;
            this.device.Clear( Xna.Color.Black );
            {
                this.RenderAction( drawContext );
            }
            this.device.SetRenderTarget( null );
            this.bloom.PostProcess( this.renderTarget, null, drawContext );
        }

        public void Draw( IDrawContext drawContext )
        {
            Draw( (IFlyDrawContext)drawContext );
        }

        private RenderTarget2D renderTarget;
        private GraphicsDevice device;

        private readonly Bloom bloom;
        private readonly IRenderTarget2DFactory renderTargetFactory;
        private readonly IGraphicsDeviceService deviceService;
    }
}
