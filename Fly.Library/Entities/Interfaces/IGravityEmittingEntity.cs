// <copyright file="IGravityEmittingEntity.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.IGravityEmittingEntity interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities
{
    using Fly.Components;

    /// <summary>
    /// Represents an <see cref="IPhysicsEntity"/> that emits gravity.
    /// </summary>
    public interface IGravityEmittingEntity : IPhysicsEntity
    {
        GravityEmitting GravityEmitter
        {
            get;
        }
    }
}
