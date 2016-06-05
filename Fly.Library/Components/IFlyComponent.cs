// <copyright file="IFlyComponent.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.IFlyComponent interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Atom.Components;
    using Fly.Entities;

    /// <summary> 
    /// An <see cref="IFlyComponent"/> represents an abstraction of specific functionality that is owned by an <see cref="IFlyEntity"/>.
    /// </summary>
    public interface IFlyComponent : IComponent
    {
        new IFlyEntity Owner
        {
            get;
        }

        void PostUpdate( IFlyUpdateContext updateContext );
    }
}
