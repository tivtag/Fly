// <copyright file="LifeTrackedEntityManager.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.UI.LifeTrackedEntityManager class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.UI
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Atom.Xna.UI;
    using Fly.Entities;

    /// <summary>
    /// Tracks entities with an ILifeStatusComponent and their corresponding LifeBar UI elements.
    /// </summary>
    public sealed class LifeTrackedEntityManager
    {
        private const int LifeBarStartY = 6;
        private const int LifeBarX = 200;
        private const int GapBetweenBars = 4;

        public LifeTrackedEntityManager( UserInterface userInterface )
        {
            Contract.Requires( userInterface != null );

            this.userInterface = userInterface;
        }

        public void Track( IFlyEntity entity )
        {
            var lifeBar = new LifeBar() { 
                Entity = entity
            };

            var trackedEntity = new LifeTrackedEntity() { FlyEntity = entity, LifeBar = lifeBar };
            this.entities.Add( trackedEntity  );
            this.OnTracked( trackedEntity );
        }

        private void OnTracked( LifeTrackedEntity entity )
        {
            this.userInterface.AddElement( entity.LifeBar );
            this.LayoutLifeBars();
        }
        
        public bool Untrack( IFlyEntity entity )
        {
            LifeTrackedEntity trackedEntity = this.entities.Find( te => te.FlyEntity == entity );

            if( this.entities.Remove( trackedEntity ) )
            {
                this.OnUntracked( trackedEntity );
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnUntracked( LifeTrackedEntity entity )
        {
            this.userInterface.RemoveElement( entity.LifeBar );
            this.LayoutLifeBars();
        }

        private void LayoutLifeBars()
        {
            float y = LifeBarStartY;

            foreach( var trackedEntity in this.entities )
            {
                var lifeBar = trackedEntity.LifeBar;

                lifeBar.Position = new Atom.Math.Vector2( LifeBarX, y );
                y += LifeBar.LifeBlockSize + GapBetweenBars;                
            }
        }

        private readonly UserInterface userInterface;
        private readonly List<LifeTrackedEntity> entities = new List<LifeTrackedEntity>();
    }
}
