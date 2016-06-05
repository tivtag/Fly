// <copyright file="ILifeStatus.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Strategy.ILifeStatus interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Strategy
{
    /// <summary>
    /// Provides a mechanism for accessing the game health status of an object.
    /// If the number of life points reach zero the object is "dead" or gets "destroyed".
    /// </summary>
    public interface ILifeStatus
    {
        int LifePoints
        {
            get;
            set;
        }

        int MaximumLifePoints
        {
            get;
            set;
        }
    }
}
