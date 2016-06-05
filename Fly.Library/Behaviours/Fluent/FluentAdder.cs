// <copyright file="FluentAdder.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Behaviours.FluentAdder{TEntity} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Behaviours
{
    using System;
    using Fly.Behaviours.Concrete;
    using Fly.Entities;
    using Fly.Graphics.Tinting;

    /// <summary>
    /// Implements the fluent configuration of the behaviour of an entity.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the entity being configured.
    /// </typeparam>
    internal sealed class FluentAdder<TEntity> : IFluentAdder<TEntity>
         where TEntity : IBehavedEntity 
    {
        public FluentAdder( TEntity entity )
        {
            this.entity = entity;
        }

        public IFluentChainer<TEntity> After( float seconds )
        {
            var behaviour = new AfterBehaviour( seconds );
            return AddChain( behaviour );
        }

        public IFluentChainer<TEntity> Spawn( Func<TEntity, IFlyEntity> entityFactory, int count = 1 )
        {
            Action<IChainBehaviour> f = b => {
                for( int i = 0; i < count; ++i )
                {
                    var parent = (TEntity)b.Entity;
                    var entity = entityFactory( parent );

                    parent.Scene.AddEntity( entity );                    
                }

                b.RemoveSelf();
            };

            var behaviour = new LambdaChainBehaviour( f );
            return AddChain( behaviour );
        }
        
        public IFluentChainer<TEntity> BlendIn( float forSeconds )
        {
            var behaviour = new TimedColorTintChainBehaviour( () => new BlendInColorTint() { TotalTime = forSeconds } );
            return AddChain( behaviour );
        }

        public IFluentChainer<TEntity> BlendOut( float forSeconds )
        {
            var behaviour = new TimedColorTintChainBehaviour( () => new BlendOutColorTint() { TotalTime = forSeconds } );
            return AddChain( behaviour );
        }

        private IFluentChainer<TEntity> AddChain( IChainBehaviour behaviour )
        {
            this.Add( behaviour );

            return new FluentChainer<TEntity>( behaviour, this );
        }
        
        private void Add( IBehaviour behaviour )
        {
            this.entity.Behaveable.AddBehaviour( behaviour );
        }

        private readonly TEntity entity;
    }
}
