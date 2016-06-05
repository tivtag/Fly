// <copyright file="IChainBehaviour.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Behaviours.IChainBehaviour interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Behaviours
{
    using Atom;

    /// <summary>
    /// Represents an <see cref="IBehaviour"/> that is chained in a double-linked list to other <see cref="IBehaviour"/>s.
    /// </summary>
    public interface IChainBehaviour : IBehaviour
    {
        event SimpleEventHandler<IChainBehaviour> Triggered;

        IBehaviour PreviousBehaviour { get; set; }
        IBehaviour NextBehaviour { get; set; }

        void RemoveSelf();
    }
}
