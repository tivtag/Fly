// <copyright file="FlyDrawContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.FlyDrawContext class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using Atom.Math;
    using Atom.Xna.Effects;
    using Fly.Graphics;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Implements the context that is passed to the Draw method of all entities, components, etc.
    /// </summary>
    /// <seealso cref="IFlyDrawable"/>
    public sealed class FlyDrawContext : Atom.Xna.SpriteDrawContext,  IFlyDrawContext
    {
        public Point2 ViewSize
        {
            get
            {
                return this.viewSizeService.ViewSize;
            }
        }

        public int ViewWidth
        {
            get
            {
                return this.viewSizeService.ViewWidth;
            }
        }

        public int ViewHeight
        {
            get
            {
                return this.viewSizeService.ViewHeight;
            }
        }

        public IShapeRenderer ShapeRenderer
        {
            get 
            {
                return this.shapeRenderer;
            }
        }

        public Camera Camera 
        {
            get; 
            set;
        }
                
        public FlyDrawContext( GraphicsDevice device, IViewSizeService viewSizeService, IEffectLoader effectLoader )
            : base( device )
        {
            this.viewSizeService = viewSizeService;

            this.shapeRenderer = new ShapeRenderer();            
            this.shapeRenderer.LoadContent( device, effectLoader );
        }

        private readonly IShapeRenderer shapeRenderer;
        private readonly IViewSizeService viewSizeService;
    }
}
