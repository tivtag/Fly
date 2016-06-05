// <copyright file="EmpireOwned.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.EmpireOwned class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Fly.Empires;

    /// <summary>
    /// Adds the mechanism of being owned by an <see cref="Fly.Empires.Empire"/> to the entity.
    /// A gathering entity that pickups a mineral will move that mineral to the total stock of minerals of the empire.
    /// </summary>
    public sealed class EmpireOwned : FlyComponent
    {
        public Empire Empire 
        { 
            get;
            set;
        }
    }
}
