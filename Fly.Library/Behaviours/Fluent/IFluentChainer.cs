// <copyright file="IFluentChainer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Behaviours.IFluentChainer{TEntity} interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Behaviours
{
    using Fly.Entities;

    /// <summary>
    /// Fluently configures the behaviour of an entity by chaining together various <see cref="IBehaviour"/>s.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the entity being configured.
    /// </typeparam>
    public interface IFluentChainer<TEntity>
         where TEntity : IBehavedEntity 
    {
        IFluentChainer<TEntity> Despawn();
        IFluentChainer<TEntity> And();
        IFluentAdder<TEntity> AndAtTheSameTime();

        IFluentChainer<TEntity> BlendOut( float forSeconds );
    }
}
