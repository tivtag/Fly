// <copyright file="PlayerUIElement.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.UI.PlayerUIElement class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.UI
{
    using Fly.Entities.Concrete;

    /// <summary>
    /// Represents an UIElement that knows about the Player object.
    /// </summary>
    public abstract class PlayerUIElement : FlyUIElement, IPlayerOwned
    {
        public Ship Player
        {
            get
            {
                return this.player;
            }

            set
            {
                this.player = value;
                this.OnPlayerChanged();
            }
        }

        protected virtual void OnPlayerChanged()
        {
        }

        private Ship player;
    }
}
