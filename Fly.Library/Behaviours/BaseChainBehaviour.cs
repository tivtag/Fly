// <copyright file="BaseChainBehaviour.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Behaviours.BaseChainBehaviour class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Behaviours
{
    using Atom;

    /// <summary>
    /// Represents the base implementation of an <see cref="IBehaviour"/> that is chained in a double-linked list to other <see cref="IBehaviour"/>s.
    /// </summary>
    public abstract class BaseChainBehaviour : BaseBehaviour, IChainBehaviour
    {
        public event SimpleEventHandler<IChainBehaviour> Triggered;
        
        public IBehaviour PreviousBehaviour
        {
            get;
            set;
        }

        public IBehaviour NextBehaviour
        {
            get;
            set;
        }

        public void ApplyChain()
        {
            this.Entity.Behaveable.RemoveBehaviour( this );
            this.Entity.Behaveable.AddBehaviour( this.NextBehaviour );
            this.Triggered.Raise( this );
        }

        public void RemoveSelf()
        {
            this.Entity.Behaveable.RemoveBehaviour( this );
        }
    }
}
