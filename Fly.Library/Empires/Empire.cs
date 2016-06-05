// <copyright file="Empire.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Empires.Empire class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Empires
{
    using Fly.Entities;

    /// <summary>
    /// An empire is a faction in the Fly world. Every empire has a home planet and stock of resources (minerals and people).
    /// </summary>
    public sealed class Empire : IMineralOwner, IPeopleOwner
    {
        public HomePlanet HomePlanet
        {
            get;
            set;
        }

        public long Minerals
        {
            get;
            private set;
        }

        public int People { get; set; }
        public int MaximumPeople { get; set; }
        
        public void AddMinerals( int count )
        {
            this.Minerals += count;
        }
    }
}
