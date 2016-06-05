// <copyright file="FlyEntity.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.FlyEntity class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities
{
    using System;
    using Atom;
    using Atom.Components;
    using Atom.Math;
    using Fly.Components;
    using Fly.Graphics;
    using Fly.Saving;

    /// <summary>
    /// Represents an object in the Fly game world. Entities are made out of multiple loosely-coupled components.
    /// </summary>
    /// <seealso cref="FlyScene"/>
    public class FlyEntity : Entity, IFlyEntity
    {
        public event RelaxedEventHandler<IFlyEntity, FlyScene> Added;
        public event RelaxedEventHandler<IFlyEntity, FlyScene> Removed;

        public IFlyEntity Parent
        {
            get
            {
                return this.transform.ParentOwner as IFlyEntity;
            }
        }

        public Vector2 Position
        {
            get
            {
                return this.transform.Position;
            }

            set
            {
                this.transform.Position = value;
            }
        }

        public float Rotation
        {
            get
            {
                return this.transform.Rotation;
            }

            set
            {
                this.transform.Rotation = value;
            }
        }

        public Transformable Transform
        {
            get
            {
                return this.transform;
            }
        }

        public Bounded Bounds
        {
            get
            {
                return this.bounded;
            }
        }

        public FlyScene Scene
        {
            get
            {
                return this.scene;
            }

            set
            {
                if( value == null )
                {
                    if( this.scene != null )
                    {
                        var scene = this.scene;
                        this.scene = null;
                        this.Removed.Raise( this, scene );
                        this.OnRemoved( scene );
                    }
                }
                else
                {
                    if( this.scene != null )
                    {
                        throw new ArgumentException( "The entity has already been added to a scene." );
                    }
                    else
                    {
                        this.scene = value;
                        this.Added.Raise( this, this.scene );
                        this.OnAdded( this.scene );
                    }
                }
            }
        }

        public IEntityDrawStrategy DrawStrategy
        {
            get
            {
                return this.drawStrategy;
            }

            set
            {
                this.drawStrategy = value;

                if( this.drawStrategy != null )
                {
                    this.drawStrategy.Entity = this;
                }
            }
        }

        public FlyEntity()
        {
            this.Components.Add( this.transform );
            this.Components.Add( this.bounded );
        }

        protected virtual void OnRemoved( FlyScene scene )
        {
        }

        protected virtual void OnAdded( FlyScene scene )
        {
        }

        public void AttachTo( FlyEntity newParent )
        {
            newParent.transform.AddChild( this.transform );
        }

        public void Draw( IFlyDrawContext drawContext )
        {
            if( this.DrawStrategy != null )
            {
                this.DrawStrategy.Draw( drawContext );
            }
        }

        public void Draw( IDrawContext drawContext )
        {
            this.Draw( (IFlyDrawContext)drawContext );
        }

        public override void Update( IUpdateContext updateContext )
        {
            if( this.drawStrategy != null )
            {
                this.drawStrategy.Update( updateContext );
            }

            base.Update( updateContext );
        }

        public void PostUpdate( IFlyUpdateContext updateContext )
        {
            foreach( var component in this.Components )
            {
                var postUpdateable = component as IFlyComponent;

                if( postUpdateable != null )
                {
                    postUpdateable.PostUpdate( updateContext );
                }
            }
        }

        public virtual void Serialize( Atom.Storage.ISerializationContext context )
        {
            context.WriteDefaultHeader();
        }

        public virtual void Deserialize( Atom.Storage.IDeserializationContext context )
        {
            context.ReadDefaultHeader( this.GetType() );
        }

        private FlyScene scene;
        private IEntityDrawStrategy drawStrategy;
        private readonly Transformable transform = new Transformable();
        private readonly Bounded bounded = new Bounded();
    }
}
