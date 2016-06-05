// <copyright file="IEntityDrawStrategy.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.IEntityDrawStrategy interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics
{
    using Fly.Entities;
    using Fly.Graphics.Tinting;
    using Fly.Saving;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Provides a mechanism for drawing an <see cref="IFlyEntity"/> in a specific way.
    /// </summary>
    public interface IEntityDrawStrategy : IFlyUpdateable, IFlyDrawable, ISaveable
    {
        ColorTintList ColorTints
        {
            get;
        }

        IFlyEntity Entity
        { 
            get;
            set;
        }

        Color Color
        {
            get;
            set;
        }
    }
}
