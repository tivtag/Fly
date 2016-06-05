// <copyright file="FlyUIElement.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.UI.FlyUIElement class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.UI
{
    using Atom;
    using Atom.Xna;
    using Atom.Xna.UI;

    /// <summary>
    /// Represents the abstract base class for <see cref="UIElement"/> that are part of a <see cref="FlyUserInterface"/>.
    /// </summary>
    public abstract class FlyUIElement : UIElement
    {
        protected override void OnDraw( ISpriteDrawContext drawContext )
        {
            this.OnDraw( (IFlyDrawContext)drawContext );
        }

        protected abstract void OnDraw( IFlyDrawContext drawContext );

        protected override void OnUpdate( IUpdateContext updateContext )
        {
            this.OnUpdate( (IFlyUpdateContext)updateContext );
        }

        protected virtual void OnUpdate( IFlyUpdateContext drawContext )
        {
        }
    }
}
