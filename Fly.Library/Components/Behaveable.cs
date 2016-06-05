// <copyright file="Behaveable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.Behaveable class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using System.Collections.Generic;
    using Fly.Behaviours;
    using Fly.Entities;

    /// <summary>
    /// Adds a mechanism of additional plugable <see cref="IBehaviour"/> to this entity.
    /// </summary>
    public sealed class Behaveable : FlyComponent
    {
        public void AddBehaviour( IBehaviour behaviour )
        {
            if( behaviour != null )
            {
                behaviour.Entity = (IBehavedEntity)this.Owner;
                this.behaviours.Add( behaviour );
            }
        }

        public override void Update( Atom.IUpdateContext updateContext )
        {
            for( int i = 0; i < this.behaviours.Count; ++i )
            {
                this.behaviours[i].Update( updateContext );                
            }
        }

        internal void RemoveBehaviour( IBehaviour behaviour )
        {
            this.behaviours.Remove( behaviour );
        }

        private readonly List<IBehaviour> behaviours = new List<IBehaviour>();
    }
}
