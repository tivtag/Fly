// <copyright file="IBreakableEntity.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.IBreakableEntity interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities
{
    using Fly.Components;

    /// <summary>
    /// Represents an <see cref="IFlyEntity"/> that can break into multiple parts.
    /// </summary>
    public interface IBreakableEntity : IFlyEntity
    {
        Breakable Breakable
        {
            get;
        }
    }
}
