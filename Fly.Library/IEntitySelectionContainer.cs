// <copyright file="IEntitySelectionContainer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.IEntitySelectionContainer interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using Fly.Entities;

    /// <summary>
    /// Represents a container for a selected IPhysicsEntity.
    /// An entity, such as the player, might interact with the selected object in various ways.
    /// </summary>
    public interface IEntitySelectionContainer
    {
        IPhysicsEntity HoveredEntity { get; }

        IPhysicsEntity SelectedEntity { get; }
    }
}
