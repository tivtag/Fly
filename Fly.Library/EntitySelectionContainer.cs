// <copyright file="EntitySelectionContainer.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.EntitySelectionContainer class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using Atom.Math;
    using Fly.Entities;
    using Fly.Graphics.Tinting;

    /// <summary>
    /// Implements a container for the IPhysicsEntity that is selected by the player.
    /// The selected IPhysicsEntity is highlighted by color tint.
    /// </summary>
    public sealed class EntitySelectionContainer : IEntitySelectionContainer
    {
        public bool DeselectOnDoubleSelection 
        {
            get; 
            set;
        }

        public IPhysicsEntity HoveredEntity
        {
            get
            {
                return this.hoveredEntity;
            }
        }

        public IPhysicsEntity SelectedEntity
        {
            get
            {
                return this.selectedEntity;
            }
        }

        public EntitySelectionContainer()
        {
            this.DeselectOnDoubleSelection = true;
        }

        public void Hover( IPhysicsEntity entity )
        {
            this.hoveredEntity = entity;
        }

        public bool SelectHovered()
        {
            return this.Select( this.hoveredEntity );
        }

        public bool Select( IPhysicsEntity entity )
        {
            if( this.selectedEntity == entity && entity != null )
            {
                if( this.DeselectOnDoubleSelection )
                {
                    this.Unselect();
                }

                return true;
            }
            else
            {
                Unselect();

                if( this.CanSelect( entity ) )
                {
                    this.selectedEntity = entity;

                    if( this.selectedEntity != null )
                    {
                        this.SelectionColorTint.Reset();
                        this.selectedEntity.DrawStrategy.ColorTints.Add( this.SelectionColorTint );
                        this.selectedEntity.Removed += this.OnSelectedEntityRemoved;

                        var breakableEntity = selectedEntity as IBreakableEntity;

                        if( breakableEntity != null )
                        {
                            breakableEntity.Breakable.Broken += this.OnSelectedEntityBroken;
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        private bool CanSelect( IPhysicsEntity entity )
        {
            var breakableEntity = entity as IBreakableEntity;

            if( breakableEntity != null )
            {
                return !breakableEntity.Breakable.IsBroken;
            }
            else
            {
                return true;
            }
        }

        private void OnSelectedEntityBroken( Components.Breakable sender )
        {
            this.Unselect();
        }

        private void OnSelectedEntityRemoved( IFlyEntity sender, FlyScene e )
        {
            this.Unselect();
        }

        public IPhysicsEntity Unselect()
        {
            if( this.selectedEntity != null )
            {
                this.selectedEntity.DrawStrategy.ColorTints.Remove( this.SelectionColorTint );
                this.selectedEntity.Removed -= this.OnSelectedEntityRemoved;

                var breakableEntity = selectedEntity as IBreakableEntity;

                if( breakableEntity != null )
                {
                    breakableEntity.Breakable.Broken -= this.OnSelectedEntityBroken;
                }
            }

            IPhysicsEntity entity = this.selectedEntity;
            this.selectedEntity = null;
            return entity;
        }

        private IPhysicsEntity selectedEntity;
        private IPhysicsEntity hoveredEntity;

        private readonly MixInColorTint SelectionColorTint = new MixInColorTint() {
            TargetColor = new Vector4( 0.8f, 0.7f, 0.0f, 0.8f ),
            TotalTime = 1.0f
        };
    }
}
