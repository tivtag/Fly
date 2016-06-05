// <copyright file="LifeBar.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.UI.LifeBar class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.UI
{
    using Atom.Math;
    using Atom.Xna;
    using Atom.Xna.Fonts;
    using Atom.Xna.UI;
    using Fly.Components;
    using Fly.Entities;
    using Fly.Strategy;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Draws the ILifeStatus of an IFlyEntity, such as the player ship or home world.
    /// </summary>
    public sealed class LifeBar : UIElement
    {
        private const int LifeBlockCount = 20;
        public const int LifeBlockSize = 12;
        private const int GapBetweenBlocks = 2;
        private static readonly Xna.Color Color = Xna.Color.Red.WithAlpha( 200 );
        private static readonly Xna.Color ColorNoLife = Xna.Color.Red.WithAlpha( 100 );

        public IFlyEntity Entity 
        {
            get 
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
                this.lifeStatus = value.Components.Find<ILifeStatusComponent>();
            }
        }

        public LifeBar()
        {
        }

        protected override void OnDraw( Atom.Xna.ISpriteDrawContext drawContext )
        {
            var batch = drawContext.Batch;

            float percentage = this.lifeStatus.LifePoints / (float)this.lifeStatus.MaximumLifePoints;
            int blockCount = (int)(percentage * LifeBlockCount);
            
            int x = (int)this.X;
            int y = (int)this.Y;

            for( int i = 0; i < blockCount; ++i )
            {
                batch.DrawRect( new Rectangle( x, y, LifeBlockSize, LifeBlockSize ), Color );
                x += LifeBlockSize + GapBetweenBlocks;
            }

            for( int i = blockCount; i < LifeBlockCount; ++i )
            {
                batch.DrawRect( new Rectangle( x, y, LifeBlockSize, LifeBlockSize ), ColorNoLife );
                x += LifeBlockSize + GapBetweenBlocks;
            }

            font.Draw( this.entity.Name, new Vector2( x + 5, y - 3 ), Color, drawContext );
        }

        protected override void OnUpdate( Atom.IUpdateContext updateContext )
        {
        }

        private IFlyEntity entity;
        private ILifeStatus lifeStatus;
        private readonly IFont font = UIFonts.Quartz10;
    }
}
