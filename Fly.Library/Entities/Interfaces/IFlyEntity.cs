// <copyright file="IFlyEntity.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.IFlyEntity interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities
{
    using Atom;
    using Atom.Components;
    using Atom.Math;
    using Fly.Components;
    using Fly.Graphics;
    using Fly.Saving;

    /// <summary>
    /// Represents an object in the Fly game world. Entities are made out of multiple loosely-coupled components.
    /// </summary>
    /// <seealso cref="FlyScene"/>
    public interface IFlyEntity : IEntity, IFlyDrawable, ISaveable
    {
        event RelaxedEventHandler<IFlyEntity, FlyScene> Added;
        event RelaxedEventHandler<IFlyEntity, FlyScene> Removed;

        Vector2 Position
        {
            get;
            set;
        }

        float Rotation
        {
            get;
            set;
        }

        Transformable Transform
        {
            get;
        }

        Bounded Bounds
        {
            get;
        }

        IEntityDrawStrategy DrawStrategy
        {
            get;
            set;
        }
        
        FlyScene Scene
        {
            get;
            set;
        }

        void PostUpdate( IFlyUpdateContext updateContext );
    }
}
