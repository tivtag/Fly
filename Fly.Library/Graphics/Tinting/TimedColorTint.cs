// <copyright file="TimedColorTint.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.Tinting.TimedColorTint class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics.Tinting
{
    using System;
    using Atom;
    using Atom.Math;

    /// <summary>
    /// Represents an <see cref="IColorTint"/> that has its effect
    /// controlled by a single time value.
    /// </summary>
    public abstract class TimedColorTint : IColorTint
    {
        /// <summary>
        /// Raised when this TimedColorTint has reached its full effect.
        /// </summary>
        public event EventHandler ReachedFullEffect;

        /// <summary>
        /// Gets or sets the total time (in seconds) for this TimedColorTint
        /// to reach its full effect.
        /// </summary>
        public float TotalTime
        {
            get
            {
                return this.totalTime;
            }

            set
            {
                this.totalTime = value;
                this.Reset();
            }
        }

        /// <summary>
        /// Gets the time left until this TimedColorTint has reached its full effect.
        /// </summary>
        protected float TimeLeft
        {
            get
            {
                return this.timeLeft;
            }
        }

        /// <summary>
        /// Gets the ratio between timeLeft and totalTime.
        /// </summary>
        /// <value>
        /// This TimedColorTint reaches its full effect when this value is 0.
        /// </value>
        protected float Factor
        {
            get
            {
                return this.timeLeft / this.totalTime;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this TimedColorTint has reached it's peak effect value.
        /// </summary>
        protected bool HasReachedFullEffect
        {
            get
            {
                return this.hasReachedFullEffect;
            }            
        }

        /// <summary>
        /// Applies this IColorTint to the given color.
        /// </summary>
        /// <param name="color">
        /// The input color.
        /// </param>
        /// <returns>
        /// The output color.
        /// </returns>
        public abstract Vector4 Apply( Vector4 color );

        /// <summary>
        /// Updates this IZeldaUpdateable.
        /// </summary>
        /// <param name="updateContext">
        /// The current ZeldaUpdateContext.
        /// </param>
        public void Update( IUpdateContext updateContext )
        {
            if( this.hasReachedFullEffect )
                return;

            this.timeLeft -= updateContext.FrameTime;

            if( this.timeLeft < 0.0f )
            {
                this.timeLeft = 0.0f;

                this.hasReachedFullEffect = true;
                this.ReachedFullEffect.Raise( this );
            }
        }

        /// <summary>
        /// Resets the time until this TimedColorTint has reached its full effect.
        /// </summary>
        public void Reset()
        {
            this.timeLeft = this.totalTime;
            this.hasReachedFullEffect = false;
        }
        
        /// <summary>
        /// The storage fields of the TotalTime property.
        /// </summary>
        private float totalTime;

        /// <summary>
        /// The time left until this TimedColorTint has reached its full effect.
        /// </summary>
        private float timeLeft;

        /// <summary>
        /// States whether this TimedColorTint has reached its full effect.
        /// </summary>
        private bool hasReachedFullEffect;
    }
}
