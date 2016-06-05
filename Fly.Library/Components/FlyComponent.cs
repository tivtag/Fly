// <copyright file="FlyComponent.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.FlyComponent class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Atom.Components;
    using Fly.Entities;

    /// <summary>
    /// An <see cref="FlyComponent"/> represents an abstraction of specific functionality that is owned by an <see cref="IFlyEntity"/>.
    /// </summary>
    public abstract class FlyComponent : Component, IFlyComponent
    {
        public new IFlyEntity Owner
        {
            get
            {
                return (IFlyEntity)base.Owner;
            }
        }

        public FlyScene Scene
        {
            get
            {
                return this.Owner.Scene;
            }
        }
        
        public virtual void PostUpdate( IFlyUpdateContext updateContext )
        {
        }
    }
}
