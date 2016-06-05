// <copyright file="Mineral.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Items.Mineral class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Items
{
    using Fly.Components;
    using Fly.Entities;

    /// <summary>
    /// Represents a Mineral item that when collected increases the mineral coiunt of the empire that owns the collector.
    /// Mineral can be used to buy and build objects.
    /// </summary>
    public sealed class Mineral : IItem
    {
        public int Count
        {
            get; 
            set; 
        }

        public bool ApplyEffectTo( IFlyEntity entity )
        {
            var empireOwned = entity.Components.Get<EmpireOwned>();
            
            if( empireOwned != null && empireOwned.Empire != null )
            {
                empireOwned.Empire.AddMinerals( this.Count );
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
