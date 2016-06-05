// <copyright file="IFluentChainer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Behaviours.IFluentAdder{TEntity} interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Behaviours
{
    using System;
    using Fly.Entities;

    /// <summary>
    /// Fluently configures the behaviour of an entity.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the entity being configured.
    /// </typeparam>
    public interface IFluentAdder<TEntity>
         where TEntity : IBehavedEntity
    {
        IFluentChainer<TEntity> After( float seconds );
        IFluentChainer<TEntity> Spawn( Func<TEntity, IFlyEntity> entityFactory, int count = 1 );

        IFluentChainer<TEntity> BlendIn( float forSeconds );
        IFluentChainer<TEntity> BlendOut( float forSeconds );        
    }
}
