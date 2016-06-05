
// <copyright file="IMultiPhysicsBodyDescription.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Physics.Descriptors.IMultiPhysicsBodyDescription interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Physics.Descriptors
{
    using System;
    using System.Collections.Generic;
    using FarseerPhysics.Dynamics;
    using Fly.Saving;

    /// <summary>
    /// Provides a mechanism for building physics objects of a specific configuration
    /// that can break into multiple other objects.
    /// </summary>
    public interface IMultiPhysicsBodyDescription : ISaveable
    {
        Tuple<IEnumerable<Fixture>, Body, object> Build( World world );
    }
}
