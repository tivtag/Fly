// <copyright file="Bounded.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.Bounded class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Atom.Components.Collision;

    /// <summary>
    /// Adds a 2D bounding collision area to the entity.
    /// The collision bounds are a simplified version of the true collision area of an entity.
    /// </summary>
    public sealed class Bounded : Collision2
    {
        public bool HasBounds
        {
            get
            {
                return this.boundCalculator != null;
            }
        }

        public override void InitializeBindings()
        {
            this.boundCalculator = this.Owner.Components.Find<IBoundCalculator>();

            base.InitializeBindings();
        }

        public void RefreshIt()
        {
            this.Refresh();
        }

        protected override void ActuallyRefreshShapes()
        {
            if( this.boundCalculator != null )
            {
                this.Rectangle = this.boundCalculator.GetBounds();
            }
        }

        protected override void Hook( Atom.Components.Transform.ITransform2 transform )
        {
            transform.Changed += this.OnTransformChanged;
        }

        protected override void Unhook( Atom.Components.Transform.ITransform2 transform )
        {
            transform.Changed -= this.OnTransformChanged;
        }

        private void OnTransformChanged( Atom.Components.Transform.ITransform2 sender )
        {
            this.ActuallyRefreshShapes();
        }

        private IBoundCalculator boundCalculator;
    }
}
