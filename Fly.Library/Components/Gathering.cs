// <copyright file="Gathering.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.Gathering class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using Fly.Items;

    /// <summary>
    /// Adds the mechanism of being able to pickup <see cref="IItem"/> to the entity.
    /// </summary>
    public sealed class Gathering : FlyComponent
    {
        public bool Gather( IItem item )
        {
            if( item.ApplyEffectTo( this.Owner ) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
