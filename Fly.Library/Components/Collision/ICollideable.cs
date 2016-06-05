// <copyright file="IIntersectable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.ICollideable interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Atom;
    using Fly.Entities;

    /// <summary>
    /// Adds a mechanism for notification about when a collision with another entity happens to the entity.
    /// </summary>
    public interface ICollideable : IFlyComponent
    {
        event RelaxedEventHandler<ICollideable, IFlyEntity> Collided;
    }
}
