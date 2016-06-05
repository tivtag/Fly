// <copyright file="SelectionInfoDisplay.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.UI.SelectionInfoDisplay class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.UI
{
    using System.Diagnostics.Contracts;
    using Atom.Math;
    using Fly.Entities;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Displays the description text of an selected entity ontop of it.
    /// </summary>
    public sealed class SelectionInfoDisplay : FlyUIElement
    {
        public SelectionInfoDisplay( IEntitySelectionContainer selectionContainer )
        {
            Contract.Requires( selectionContainer != null );

            this.selectionContainer = selectionContainer;
        }

        protected override void OnDraw( IFlyDrawContext drawContext )
        {
            if( selectionContainer.SelectedEntity != null )
            {
                var entity = selectionContainer.SelectedEntity;

                Vector2 position = drawContext.Camera.Project( GetPosition( entity ) );
                position.X = (int)System.Math.Floor( position.X );
                position.Y = (int)System.Math.Floor( position.Y );

                string text = GetText( entity );
                UIFonts.Tahoma10.Draw( text, position, Xna.Color.LightGreen, drawContext );
            }
        }

        private Vector2 GetPosition( IPhysicsEntity entity )
        {
            return entity.Physics.Center;
        }

        private static string GetText( IFlyEntity entity )
        {
            var described = entity as IDescribed;

            string text;
            if( described != null )
            {
                text = described.Description;
            }
            else
            {
                text = entity.Name;
            }

            if( text == null )
            {
                text = entity.ToString();
            }

            return text;
        }

        private readonly IEntitySelectionContainer selectionContainer;
    }
}
