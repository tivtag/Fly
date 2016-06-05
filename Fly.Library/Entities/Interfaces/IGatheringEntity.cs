// <copyright file="IGatheringEntity.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.IGatheringEntity interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities
{
    using Fly.Components;

    /// <summary>
    /// Represents an <see cref="IFlyEntity"/> that can gather other entities.
    /// </summary>
    public interface IGatheringEntity : IFlyEntity
    {
        Gathering Gathering
        {
            get;
        }
    }
}
