// <copyright file="IDescribed.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.IDescribed interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    /// <summary>
    /// Provides a mechanism for receiving a description text.
    /// </summary>
    public interface IDescribed
    {
        string Description
        {
            get;
        }
    }
}
