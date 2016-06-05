// <copyright file="IFlyUpdateable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.IFlyUpdateable interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using Atom;

    /// <summary>
    /// When implemented allows the objects to be updated during every frame in the Fly world.
    /// </summary>
    public interface IFlyUpdateable : IUpdateable
    {
        void Update( IFlyUpdateContext updateContext );
    }
}
