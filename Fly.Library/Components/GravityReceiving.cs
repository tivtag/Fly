// <copyright file="GravityReceiving.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.GravityReceiving class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    /// <summary>
    /// Adds the mechanism of being able to receive the force of gravity to the entity.
    /// </summary>
    public sealed class GravityReceiving : FlyComponent
    {
        public float Factor 
        {
            get;
            set;
        }

        public GravityReceiving()
        {
            this.Factor = 1.0f;
        }
    }
}
