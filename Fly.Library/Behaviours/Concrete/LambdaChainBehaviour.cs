// <copyright file="LambdaChainBehaviour.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Behaviours.Concrete.LambdaChainBehaviour class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Behaviours.Concrete
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// A behaviour that executes a general purpose behaviour.
    /// </summary>
    public sealed class LambdaChainBehaviour : BaseChainBehaviour
    {
        public LambdaChainBehaviour( Action<IChainBehaviour> action )
        {
            Contract.Requires( action != null );

            this.action = action;
        }

        public override void Update( IFlyUpdateContext updateContext )
        {
            this.action( this );
        }

        private readonly Action<IChainBehaviour> action;
    }
}
