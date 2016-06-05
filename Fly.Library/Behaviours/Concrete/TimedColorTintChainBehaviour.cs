// <copyright file="TimedColorTintChainBehaviour.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Behaviours.Concrete.TimedColorTintChainBehaviour class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Behaviours.Concrete
{
    using System;
    using Fly.Graphics.Tinting;

    /// <summary>
    /// A behaviour that adds a <see cref="TimedColorTint"/> to the entity.
    /// The next behaviour in the chain is applied when the TimedColorTint ends.
    /// </summary>
    public sealed class TimedColorTintChainBehaviour : BaseChainBehaviour
    {
        public TimedColorTintChainBehaviour( Func<TimedColorTint> tintFactory )
        {
            this.tintFactory = tintFactory;
        }

        protected override void OnEntityChanged()
        {
            TimedColorTint tint = this.tintFactory();
            tint.ReachedFullEffect += ( sender, e ) => this.ApplyChain();

            this.Entity.DrawStrategy.ColorTints.Add( tint );
        }

        public override void Update( IFlyUpdateContext updateContext )
        {
        }

        private readonly Func<TimedColorTint> tintFactory;
    }
}
