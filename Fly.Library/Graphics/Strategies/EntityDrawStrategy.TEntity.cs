// <copyright file="EntityDrawStrategy.TEntity.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.Strategies.EntityDrawStrategy{TEntity} class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics.Strategies
{
    using Atom.Xna;
    using Fly.Entities;
    using Fly.Graphics.Tinting;
    using Fly.Saving;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The base implementation of the <see cref="IEntityDrawStrategy"/> for entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the entity.
    /// </typeparam>
    public abstract class EntityDrawStrategy<TEntity> : IEntityDrawStrategy, ISaveable
        where TEntity : IFlyEntity
    {
        public ColorTintList ColorTints
        {
            get
            {
                return this.colorTints;
            }
        }

        public TEntity Entity
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

        IFlyEntity IEntityDrawStrategy.Entity
        {
            get
            {
                return this.Entity;
            }
            set
            {
                this.Entity = (TEntity)value;
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public Color FinalColor
        {
            get
            {
                return this.ColorTints.ApplyTinting( this.color );
            }
        }

        protected virtual void OnEntityChanged()
        {
        }

        public virtual void Update( IFlyUpdateContext updateContext )
        {
            this.colorTints.Update( updateContext );
        }

        public void Update( Atom.IUpdateContext updateContext )
        {
            this.Update( (IFlyUpdateContext)updateContext );
        }

        public abstract void Draw( IFlyDrawContext drawContext );

        public void Draw( Atom.IDrawContext drawContext )
        {
            this.Draw( (IFlyDrawContext)drawContext );
        }

        public void Serialize( Atom.Storage.ISerializationContext context )
        {
            context.WriteDefaultHeader();
            context.Write( color );
        }

        public void Deserialize( Atom.Storage.IDeserializationContext context )
        {
            context.ReadDefaultHeader( typeof( EntityDrawStrategy<TEntity> ) );
            color = context.ReadColor();
        }

        protected Color ApplyTinting( Color color )
        {
            return this.ColorTints.ApplyTinting( color );
        }

        private TEntity entity;
        private Color color = Color.Red;
        private readonly ColorTintList colorTints = new ColorTintList();
    }
}
