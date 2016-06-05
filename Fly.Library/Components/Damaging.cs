// <copyright file="Damaging.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.Damaging class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Fly.Entities;
    using Atom;

    /// <summary>
    /// Adds a mechanism for being able to damage other entities to this entity.
    /// </summary>
    public sealed class Damaging : FlyComponent
    {
        public event RelaxedEventHandler<Damaging, IFlyEntity> DamagedOther;

        public int Damage 
        {
            get; 
            set;
        }

        public Damaging()
        {
            this.Damage = 1;
        }

        public void OnDamaged( IFlyEntity entity )
        {
            this.DamagedOther.Raise( this, entity );
        }
    }
}
