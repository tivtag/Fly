// <copyright file="IGravityReceivingEntity.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.IGravityReceivingEntity interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities
{
    using Fly.Components;

    /// <summary>
    /// Represents an <see cref="IPhysicsEntity"/> that receives gravity.
    /// </summary>
    public interface IGravityReceivingEntity : IPhysicsEntity
    {
        GravityReceiving GravityReceiver
        {
            get;
        }
    }
}
