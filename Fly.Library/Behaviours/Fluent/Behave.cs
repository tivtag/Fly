// <copyright file="Behave.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Behaviours.Behave class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Behaviours
{
    using Fly.Entities;

    /// <summary>
    /// Configures the behaviour of an <see cref="IBehavedEntity"/>.
    /// </summary>
    public static class Behave
    {
        public static IFluentAdder<TEntity> This<TEntity>( TEntity entity )
            where TEntity : IBehavedEntity
        {
            return new FluentAdder<TEntity>( entity );
        }
    }
}
