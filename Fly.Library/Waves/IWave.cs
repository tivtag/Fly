// <copyright file="IWave.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Waves.IWave interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Waves
{
    /// <summary>
    /// A wave is a game event in which multiple things can happen; such as enemy invasion starting.
    /// </summary>
    /// <seealso cref="IWaveGuide"/>
    public interface IWave
    {
        string ContentDescription 
        { 
            get;  
        }

        void SpawnIn( GameWorld world );
    }
}
