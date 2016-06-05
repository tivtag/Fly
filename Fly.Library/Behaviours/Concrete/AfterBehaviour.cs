// <copyright file="AfterBehaviour.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Behaviours.Concrete.AfterBehaviour class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Behaviours.Concrete
{
    /// <summary>
    /// A behaviour that takes a pause of N seconds before the next behaviour in the chain is executed.
    /// </summary>
    public sealed class AfterBehaviour : BaseChainBehaviour
    {
        public AfterBehaviour( float seconds )
        {
            this.seconds = seconds;
        }

        public override void Update( IFlyUpdateContext updateContext )
        {
            this.seconds -= updateContext.FrameTime;

            if( this.seconds <= 0.0f )
            {
                this.ApplyChain();
            }
        }

        private float seconds;
    }
}
