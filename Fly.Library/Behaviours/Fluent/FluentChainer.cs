// <copyright file="FluentChainer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Behaviours.FluentChainer{TEntity} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Behaviours
{
    using Fly.Behaviours.Concrete;
    using Fly.Entities;
    using Fly.Graphics.Tinting;

    /// <summary>
    /// Implements the fluent configuration of the behaviour of an entity by chaining together various <see cref="IBehaviour"/>s.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the entity being configured.
    /// </typeparam>
    internal sealed class FluentChainer<TEntity> : IFluentChainer<TEntity>
         where TEntity : IBehavedEntity 
    {
        internal FluentChainer( IChainBehaviour behaviour, IFluentAdder<TEntity> fluentAdder )
        {
            this.behaviour = behaviour;
            this.fluentAdder = fluentAdder;
        }

        public IFluentChainer<TEntity> Despawn()
        {            
            return this.Chain( new LambdaChainBehaviour( b => {
                var entity = b.Entity;

                if( entity.Scene != null )
                {
                    entity.Scene.RemoveEntity( entity );
                }
            } ) );
        }

        public IFluentChainer<TEntity> BlendOut( float forSeconds )
        {
            return this.Chain( new TimedColorTintChainBehaviour( () => new BlendOutColorTint() { TotalTime = forSeconds } ) );
        }

        public IFluentChainer<TEntity> And()
        {
            return this;
        }

        public IFluentAdder<TEntity> AndAtTheSameTime()
        {
            return this.fluentAdder;
        }

        private IFluentChainer<TEntity> Chain( IChainBehaviour nextBehaviour )
        {
            this.behaviour.NextBehaviour = nextBehaviour;
            nextBehaviour.PreviousBehaviour = this.behaviour;

            return new FluentChainer<TEntity>( nextBehaviour, this.fluentAdder );
        }

        private readonly IFluentAdder<TEntity> fluentAdder;
        private readonly IChainBehaviour behaviour;
    }
}
