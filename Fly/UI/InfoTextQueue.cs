// <copyright file="InfoTextQueue.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.UI.InfoTextQueue class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.UI
{
    using Atom.Xna.UI;

    /// <summary>
    /// Represents an UIElement that allows one to show <see cref="InfoText"/> to the player.
    /// </summary>
    public sealed class InfoTextQueue : UIElement
    {
        public void Queue( string header, string text )
        {
            this.text = new InfoText() {
                Header = header,
                Text = text
            };

            this.text.Hidden += ( sender, e ) => {
                this.text = null;
            };
        }

        protected override void OnDraw( Atom.Xna.ISpriteDrawContext drawContext )
        {
            if( this.text != null )
            {
                this.text.Draw( (IFlyDrawContext)drawContext );
            }
        }

        protected override void OnUpdate( Atom.IUpdateContext updateContext )
        {
            if( this.text != null )
            {
                this.text.Update( updateContext );
            }
        }

        private InfoText text;
    }
}
