// <copyright file="IPeopleOwner.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Empires.IPeopleOwner interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Empires
{
    /// <summary>
    /// Represents an object that owns a number of people. E.g. has a population.
    /// </summary>
    public interface IPeopleOwner
    {
        int People { get; set; }

        int MaximumPeople { get; set; }
    }
}
