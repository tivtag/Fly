// <copyright file="IngameState.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.States.IngameUserInterface class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.UI
{
    using System.Linq;
    using Fly.Entities;
    using Fly.Entities.Concrete;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// Represents the User Interface that is shown when the player is playing the game.
    /// </summary>
    public sealed class IngameUserInterface : FlyUserInterface
    {
        public LifeTrackedEntityManager TrackedEntities
        {
            get
            {
                return this.trackedEntities;
            }
        }

        public IngameUserInterface()
            : base( 10 )
        {
            this.trackedEntities = new LifeTrackedEntityManager( this );

            this.AddElement( new RotationIndicator() );
            this.AddElement( new IngameInfoBar() );

            this.KeyboardInput += this.OnKeyboardInput;
        }

        public void Setup( Ship player )
        {
            foreach( IPlayerOwned element in this.Elements.Where( e => e is IPlayerOwned ).Cast<IPlayerOwned>() )
            {
                element.Player = player;
            }
        }

        public void TrackEntity( IFlyEntity entity )
        {
            this.trackedEntities.Track( entity );
        }

        private void OnKeyboardInput( object sender, ref KeyboardState keyState, ref KeyboardState oldKeyState )
        {
            if( keyState.IsKeyDown( Keys.C ) && oldKeyState.IsKeyUp( Keys.C ) )
            {
                var rotationIndicator = this.GetElement<RotationIndicator>();
                rotationIndicator.IsLarge = !rotationIndicator.IsLarge;
            }
        }

        private readonly LifeTrackedEntityManager trackedEntities;
    }
}
