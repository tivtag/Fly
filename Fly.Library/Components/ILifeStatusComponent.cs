// <copyright file="ILifeStatusComponent.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.ILifeStatusComponent interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Fly.Strategy;

    /// <summary>
    /// Representa a component that adds a life/health status to an entity; allowing it to be damaged by other <see cref="IDamageEntity"/>s.
    /// </summary>
    /// <seealso cref="LifeStatusComponent"/>
    public interface ILifeStatusComponent : ILifeStatus, IFlyComponent
    {
    }
}
