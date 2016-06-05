// <copyright file="SceneGravityCalculator.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Physics.SceneGravityApplicator class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Physics
{
    using System.Diagnostics.Contracts;
    using Fly.Entities;

    /// <summary>
    /// Applies the gravity effect from all gravity emitting entities of a scene onto all gravity receiving entities.
    /// </summary>
    public sealed class SceneGravityApplicator
    {
        public SceneGravityApplicator( FlyScene scene )
        {
            Contract.Requires( scene != null );

            this.scene = scene;
        }

        public void Apply()
        {
            foreach( IGravityEmittingEntity emittingEntity in this.scene.GravityEmittingEntities )
            {
                if( !emittingEntity.GravityEmitter.IsEnabled )
                {
                    continue;
                }

                foreach( IGravityReceivingEntity receivingEntity in this.scene.GravityReceivingEntities )
                {
                    if( receivingEntity != emittingEntity &&
                        receivingEntity.GravityReceiver.IsEnabled )
                    {
                        emittingEntity.GravityEmitter.ApplyGravityTo( receivingEntity );
                    }
                }
            }
        }

        private readonly FlyScene scene;
    }
}
