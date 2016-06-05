// <copyright file="IMineralOwner.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Empires.IMineralOwner interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Empires
{
    /// <summary>
    /// Represents an object that owns minerals.
    /// Minerals are a resource that allows one to aquire, research and build new objects.
    /// </summary>
    public interface IMineralOwner
    {
        long Minerals 
        { 
            get; 
        }

        void AddMinerals( int count );
    }
}
