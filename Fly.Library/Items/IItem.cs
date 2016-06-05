// <copyright file="IItem.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Items.IItem interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Items
{
    using Fly.Entities;

    /// <summary>
    /// Represents an item that when used on an entity, such as the player, has a specific effect.
    /// </summary>
    public interface IItem
    {
        bool ApplyEffectTo( IFlyEntity entity );
    }
}
