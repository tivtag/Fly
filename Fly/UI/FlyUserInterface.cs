// <copyright file="FlyUserInterface.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.UI.FlyUserInterface class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.UI
{
    using Atom.Xna.UI;

    /// <summary>
    /// Represents the abstract base class for the <see cref="UserInterface"/>s
    /// that are part of the Fly game.
    /// An UI presents the user with visual information and interaction points with the application.
    /// </summary>
    public abstract class FlyUserInterface : UserInterface
    {
        protected FlyUserInterface( int elementCapacity )
            : base( elementCapacity )
        {
            this.infoTextQueue = new InfoTextQueue();

            this.AddElement( infoTextQueue );
        }

        public void ShowInfo( string header, string text )
        {
            this.infoTextQueue.Queue( header, text );
        }

        private readonly InfoTextQueue infoTextQueue;
    }
}
