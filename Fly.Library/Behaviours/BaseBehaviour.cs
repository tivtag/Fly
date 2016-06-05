// <copyright file="BaseBehaviour.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Behaviours.BaseBehaviour class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Behaviours
{
    using Fly.Entities;

    /// <summary>
    /// Represents the base implementation of an <see cref="IBehaviour"/>.
    /// </summary>
    public abstract class BaseBehaviour : IBehaviour
    {
        public IBehavedEntity Entity
        {
            get
            {
                return this.entity;
            }

            set
            {
                this.entity = value;
                this.OnEntityChanged();
            }
        }

        protected virtual void OnEntityChanged()
        {
        }

        public abstract void Update( IFlyUpdateContext updateContext );

        public void Update( Atom.IUpdateContext updateContext )
        {
            this.Update( (IFlyUpdateContext)updateContext );
        }

        private IBehavedEntity entity;
    }
}
