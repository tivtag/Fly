// <copyright file="AsteroidPusher.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.AsteroidPusher class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using System.Diagnostics.Contracts;
    using Atom.Math;
    using Fly.Entities.Concrete;

    /// <summary>
    /// Randomly pushes <see cref="Asteroid"/> instances in a FlyScene to get them moving.
    /// </summary>
    public sealed class AsteroidPusher
    {
        public AsteroidPusher( IRand rand )
        {
            Contract.Requires( rand != null );

            this.rand = rand;
        }

        public void Push( FlyScene scene )
        {
            foreach( Asteroid asteroid in scene.GetEntities<Asteroid>() )
            {
                if( ShouldPush() )
                {
                    asteroid.Physics.LinearVelocity = GetLinearVelocity();
                }

                if( ShouldRotate() )
                {
                    asteroid.Physics.AngularVelocity = GetAngularVelocity();
                }
            }
        }

        private bool ShouldRotate()
        {
            return this.rand.UncheckedRandomRange( 0, 100 ) <= 65;
        }

        private bool ShouldPush()
        {
            return this.rand.UncheckedRandomRange( 0, 100 ) <= 51;
        }

        private Vector2 GetLinearVelocity()
        {
            const int AsteroidSpeed = 2;
            return new Vector2( rand.UncheckedRandomRange( -AsteroidSpeed, AsteroidSpeed ), rand.UncheckedRandomRange( -AsteroidSpeed, AsteroidSpeed ) );
        }

        private float GetAngularVelocity()
        {
            return rand.UncheckedRandomRange( -1.0f, 1.0f );
        }

        private readonly IRand rand;
    }
}
