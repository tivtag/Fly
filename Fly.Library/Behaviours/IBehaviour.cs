// <copyright file="IBehaviour.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Behaviours.IBehaviour interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Behaviours
{
    using Fly.Entities;

    /// <summary>
    /// A behaviour adds game logic to an entity that gets updated every frame.
    /// </summary>
    public interface IBehaviour : IFlyUpdateable
    {
        IBehavedEntity Entity 
        {
            get; 
            set;
        }
    }
}
