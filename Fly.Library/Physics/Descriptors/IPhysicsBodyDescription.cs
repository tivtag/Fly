// <copyright file="IPhysicsBodyDescription.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Physics.Descriptors.IPhysicsBodyDescription interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Physics.Descriptors
{
    using FarseerPhysics.Dynamics;
    using Fly.Saving;

    /// <summary>
    /// Provides a mechanism for building physics objects of a specific configuration.
    /// </summary>
    public interface IPhysicsBodyDescription : ISaveable
    {
        Body Build( World world );
    }
}
