// <copyright file="FlyScene.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.FlyScene class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Atom.Math;
    using Atom.Xna;
    using FarseerPhysics.Collision;
    using FarseerPhysics.Dynamics;
    using Fly.Components;
    using Fly.Entities;
    using XnaF = Microsoft.Xna.Framework;

    /// <summary>
    /// Aggregates the state of the game <see cref="World"/> with all it's <see cref="IFlyEntity"/>s.
    /// </summary>
    public sealed class FlyScene : IFlyUpdateable
    {
        public int EntityCount
        {
            get
            {
                return this.entities.Count;
            }
        }

        public World PhysicsWorld
        {
            get
            {
                return this.physicsWorld;
            }
        }

        public Point2 HalfSize
        {
            get
            {
                return this.halfSize;
            }
        }

        public IEnumerable<IFlyEntity> Entities
        {
            get
            {
                return entities;
            }
        }

        public IEnumerable<IGravityReceivingEntity> GravityReceivingEntities
        {
            get
            {
                return this.gravityReceivingEntities;
            }
        }

        public IEnumerable<IGravityEmittingEntity> GravityEmittingEntities
        {
            get
            {
                return this.gravityEmittingEntities;
            }
        }

        public int HiddenEntities
        {
            get;
            set;
        }

        public FlyScene( Point2 halfSize )
        {
            this.halfSize = halfSize;
            this.physicsWorld = new World( new XnaF.Vector2( 0.0f, 0.0f ) );
        }

        public void AddEntities( IEnumerable<IFlyEntity> entities )
        {
            foreach( IFlyEntity entity in entities )
            {
                this.AddEntity( entity );
            }
        }

        public void AddEntity( IFlyEntity entity )
        {
            if( entity != null )
            {
                entity.Scene = this;
                this.entities.Add( entity );

                var gravityEmitting = entity as IGravityEmittingEntity;
                var gravityReceiving = entity as IGravityReceivingEntity;

                if( gravityEmitting != null )
                {
                    this.gravityEmittingEntities.Add( gravityEmitting );
                }

                if( gravityReceiving != null )
                {
                    this.gravityReceivingEntities.Add( gravityReceiving );
                }

                entity.Bounds.RefreshIt();
            }
        }

        public void RemoveEntity( IFlyEntity entity )
        {
            if( this.entities.Remove( entity ) )
            {
                entity.Scene = null;

                var gravityEmitting = entity as IGravityEmittingEntity;
                var gravityReceiving = entity as IGravityReceivingEntity;

                if( gravityEmitting != null )
                {
                    this.gravityEmittingEntities.Remove( gravityEmitting );
                }

                if( gravityReceiving != null )
                {
                    this.gravityReceivingEntities.Remove( gravityReceiving );
                }
            }
        }

        public IEnumerable<TEntity> GetEntities<TEntity>()
            where TEntity : IFlyEntity
        {
            return this.entities.Where( e => e is TEntity ).Cast<TEntity>();
        }

        public void Update( IFlyUpdateContext updateContext )
        {
            for( int i = 0; i < this.entities.Count; ++i )
            {
                entities[i].PreUpdate( updateContext );
            }

            for( int i = 0; i < entities.Count; ++i )
            {
                entities[i].Update( updateContext );
            }

            float stepTime = Math.Min( (float)updateContext.GameTime.ElapsedGameTime.TotalMilliseconds * 0.001f, (1f / 30f) );
            this.physicsWorld.Step( stepTime );

            for( int i = 0; i < this.entities.Count; ++i )
            {
                entities[i].PostUpdate( updateContext );
            }
        }

        public IPhysicsEntity GetPhysicsEntityAt( Vector2 point )
        {
            AABB aabb = new AABB( point.ToXna(), 0.1f, 0.1f );
            IIntersectable component = null;

            this.physicsWorld.QueryAABB( fixture => {
                    component = fixture.Body.UserData as IIntersectable;

                    if( component != null )
                    {
                        if( component.IntersectsAt( point ) )
                        {
                            return false;
                        }
                    }

                    return true;
                },
                ref aabb
            );

            return component != null ? (component.Owner as IPhysicsEntity) : null;
        }

        //public IPhysicsEntity GetPhysicsEntityIn( RectangleF area )
        //{
        //    AABB aabb = new AABB( new XnaF.Vector2( area.Width, area.Height ), area.Position.ToXna() );
        //    IFlyComponent component = null;

        //    Console.WriteLine( "query" );

        //    this.physicsWorld.QueryAABB( fixture => {
        //        component = fixture.Body.UserData as ICollideable;

        //        if( component != null )
        //        {
        //            var bounds = component.Owner.Bounds;
        //            if( bounds.Rectangle.Intersects( area ) )
        //            {
        //                return false;
        //            }
        //        }

        //        return true;
        //    },
        //        ref aabb
        //    );

        //    return component != null ? (component.Owner as IPhysicsEntity) : null;
        //}

        public void Draw( IFlyDrawContext drawContext )
        {
            HiddenEntities = 0;
            RectangleF area = drawContext.Camera.Area;

            for( int i = 0; i < this.entities.Count; ++i )
            {
                IFlyEntity entity = this.entities[i];

                if( !entity.Bounds.HasBounds || entity.Bounds.Intersects( ref area ) )
                {
                    entity.Draw( drawContext );
                }
                else
                {
                    ++HiddenEntities;
                }
            }
        }

        public void Update( Atom.IUpdateContext updateContext )
        {
            this.Update( (IFlyUpdateContext)updateContext );
        }

        private readonly Point2 halfSize;

        private readonly List<IFlyEntity> entities = new List<IFlyEntity>();
        private readonly List<IGravityEmittingEntity> gravityEmittingEntities = new List<IGravityEmittingEntity>();
        private readonly List<IGravityReceivingEntity> gravityReceivingEntities = new List<IGravityReceivingEntity>();

        private readonly World physicsWorld;
    }
}
