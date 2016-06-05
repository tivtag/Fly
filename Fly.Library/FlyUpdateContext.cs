// <copyright file="FlyUpdateContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.FlyUpdateContext class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using Atom.Xna;

    /// <summary>
    /// The context that is passed to the Update method of all entities, components, etc.
    /// </summary>
    public sealed class FlyUpdateContext : XnaUpdateContext, IFlyUpdateContext
    {
    }
}
