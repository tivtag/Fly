// <copyright file="IPlayerOwned.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.UI.IPlayerOwned interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.UI
{
    using Fly.Entities.Concrete;

    /// <summary>
    /// Represents an object that is owned by the player.
    /// </summary>
    public interface IPlayerOwned
    {
        Ship Player { get; set; }
    }
}
