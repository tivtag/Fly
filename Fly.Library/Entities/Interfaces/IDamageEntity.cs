// <copyright file="IDamageEntity.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.IDamageEntity interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities
{
    using Fly.Components;

    /// <summary>
    /// Represents an <see cref="IFlyEntity"/> that can deal damage to other entities.
    /// </summary>
    public interface IDamageEntity : IFlyEntity
    {
        Damaging Damaging { get; }
    }
}
