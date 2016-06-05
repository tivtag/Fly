// <copyright file="EntityDrawStrategy.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.Strategies.EntityDrawStrategy class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics.Strategies
{
    using Fly.Entities;

    /// <summary>
    /// The base implementation of the <see cref="IEntityDrawStrategy"/> for entities of type <see cref="IFlyEntity"/>.
    /// </summary>
    public abstract class EntityDrawStrategy : EntityDrawStrategy<IFlyEntity>
    {
    }
}
