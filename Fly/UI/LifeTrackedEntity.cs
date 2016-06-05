// <copyright file="LifeTrackedEntity.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.UI.LifeTrackedEntity class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.UI
{
    using Fly.Entities;

    /// <summary>
    /// Aggregates an entity with an ILifeStatusComponent and it's corresponding LifeBar UI Element.
    /// </summary>
    internal sealed class LifeTrackedEntity
    {
        public IFlyEntity FlyEntity { get; set; }

        public LifeBar LifeBar { get; set; }
    }
}
