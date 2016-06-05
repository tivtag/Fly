// <copyright file="IFlyUpdateContext.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.IFlyUpdateContext interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using Atom.Xna;

    /// <summary>
    /// Represents the context that is passed to the Update method of all entities, components, etc.
    /// </summary>
    /// <seealso cref="IFlyUpdateable"/>
    public interface IFlyUpdateContext : IXnaUpdateContext
    {
    }
}
