// <copyright file="FixedIntervalWaveGuide.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Waves.FixedIntervalWaveGuide class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Waves
{
    using Atom;
    using Atom.Math;

    /// <summary>
    /// Implements an <seealso cref="IWaveGuide"/> that launches new <seealso cref="IWave"/>s in a fixed time interval.
    /// </summary>
    public sealed class FixedIntervalWaveGuide : IWaveGuide
    {
        private const float TimeInSecondsBetweenTwoWaves = 60.0f;

        public event RelaxedEventHandler<IWave> Launched = delegate { };

        public FixedIntervalWaveGuide( GameWorld world, IRand rand )
        {
            this.world = world;
            this.rand = rand;
        }

        public void Update( IFlyUpdateContext updateContext )
        {
            this.timeLeft -= updateContext.FrameTime;

            if( this.timeLeft <= 0.0f )
            {
                this.Trigger();
            }
        }

        public void Update( IUpdateContext updateContext )
        {
            Update( (IFlyUpdateContext)updateContext );
        }

        public void Trigger()
        {
            ++this.count;
            this.timeLeft = TimeInSecondsBetweenTwoWaves;

            IWave wave = this.CreateWave();
            wave.SpawnIn( this.world );
            this.Launched( this, wave );
        }

        private IWave CreateWave()
        {
            return new KamikazeBlubWave( this.rand );
        }

        private float timeLeft = TimeInSecondsBetweenTwoWaves;
        private int count;

        private readonly GameWorld world;
        private readonly IRand rand;
    }
}
