// <copyright file="IBehavedEntity.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.IBehavedEntity interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities
{
    using Fly.Components;

    /// <summary>
    /// Represents an <see cref="IFlyEntity"/> that behaves in a certain (automatic) way.
    /// </summary>
    public interface IBehavedEntity : IFlyEntity
    {
        Behaveable Behaveable
        {
            get;
        }
    }
}
