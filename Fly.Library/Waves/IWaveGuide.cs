// <copyright file="IWaveGuide.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Waves.IWaveGuide interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Waves
{
    /// <summary>
    /// Provides a mechanism for launching <seealso cref="IWave"/>s onto the game world.
    /// </summary>
    public interface IWaveGuide : IFlyUpdateable
    {
        event Atom.RelaxedEventHandler<IWave> Launched;

        void Trigger();
    }
}
