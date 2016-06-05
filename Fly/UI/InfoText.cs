// <copyright file="InfoText.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.UI.InfoText class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.UI
{
    using System;
    using Atom;
    using Atom.Math;
    using Atom.Xna.Effects;
    using Atom.Xna.Fonts;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Represents an informational text that is shown to the player for a given duration.
    /// </summary>
    public sealed class InfoText
    {
        public event EventHandler Hidden;

        public string Header { get; set; }

        public string Text { get; set; }

        public InfoText( float duration = 5.0f )
        {
            this.totalTime = duration;
            this.timeLeft = duration;
            this.blendEffect = new AlphaBlendInOutColorEffect( duration, 1.0f, duration - 1.0f );
        }

        public void Update( IUpdateContext updateContext )
        {
            this.timeLeft -= updateContext.FrameTime;
            this.blendEffect.Update( updateContext );

            if( this.timeLeft <= 0.0f )
            {
                this.Hidden.Raise( this );
            }
        }

        public void Draw( IFlyDrawContext drawContext )
        {
            var center = drawContext.ViewSize / 2;

            UIFonts.Quartz14.Draw( this.Header, center, TextAlign.Center, this.blendEffect.Apply( Xna.Color.Red ), drawContext );
            UIFonts.Quartz10.Draw( this.Text, center + new Vector2( 0.0f, UIFonts.Quartz14.LineSpacing + 10 ), TextAlign.Center, this.blendEffect.Apply( Xna.Color.White ), drawContext );
        }

        private float totalTime;
        private float timeLeft;

        private readonly AlphaBlendInOutColorEffect blendEffect;
    }
}
