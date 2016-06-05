// <copyright file="Camera.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Camera class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using Atom;
    using Fly.Entities;
    using Fly.Graphics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The view port that represents the part of the game world that is visible.
    /// </summary>
    public sealed class Camera
    {
        private const float MinimumZoom = 0.05f;
        private const float ZoomTime = 0.85f;
        
        public Matrix Projection;
        public Matrix View = Matrix.Identity;
        public Atom.Math.RectangleF Area;

        public float Zoom
        {
            get
            {
                return this.zoom;
            }

            set
            {
                this.zoom = CrampZoom( value );
                this.RefreshProjection();
                this.RefreshArea();
            }
        }

        public IFlyEntity EntityToFollow
        {
            get
            {
                return this.entityToFollow;
            }

            set
            {
                if( this.entityToFollow != null )
                {
                    this.entityToFollow.Transform.PositionChanged -= this.OnPositionChanged;
                }

                this.entityToFollow = value;

                if( this.entityToFollow != null )
                {
                    this.entityToFollow.Transform.PositionChanged += this.OnPositionChanged;
                }

                this.RefreshView();
            }
        }

        public Atom.Math.Point2 ViewSize
        {
            get
            {
                return this.viewSizeService.ViewSize;
            }
        }

        public Atom.Math.Vector2 Scroll
        {
            get
            {
                return scroll;
            }

            set
            {
                scroll = value;
                RefreshView();
            }
        }  

        public Camera( IViewSizeService viewSizeService, GraphicsDevice device )
        {
            this.viewSizeService = viewSizeService;
            this.device = device;

            this.RefreshProjection();
            this.RefreshView();
        }

        private void OnPositionChanged( object sender, ChangedValue<Atom.Math.Vector2> e )
        {
            this.RefreshView();
        }

        private void RefreshProjection()
        {
            Viewport viewport = this.device.Viewport;

            const float ScaleFactor = 40.0f;
            float aspect = (float)viewport.Width / (float)viewport.Height;
            this.Projection = Matrix.CreateOrthographic( ScaleFactor * aspect * this.zoom, ScaleFactor * this.zoom, 0, 1 );
        }

        private void RefreshView()
        {
            if( this.EntityToFollow != null )
            {
                this.scroll = this.EntityToFollow.Position;
                this.View = Matrix.CreateTranslation( -this.Scroll.X, -this.Scroll.Y, 0.0f );
            }
            else
            {
                this.View = Matrix.CreateTranslation( -this.Scroll.X, -this.Scroll.Y, 0.0f );
            }

            this.RefreshArea();
        }

        private void RefreshArea()
        {
            var translation = Unproject( Atom.Math.Vector2.Zero );
            var size = UnprojectIdentity( viewSizeService.ViewSize );

            this.Area = new Atom.Math.RectangleF( new Atom.Math.Vector2( translation.X, translation.Y + size.Y * 2 ), new Atom.Math.Vector2( size.X * 2, -size.Y * 2 ) );
        }

        public Atom.Math.Vector2 Unproject( Atom.Math.Vector2 point )
        {
            Vector3 projected = this.device.Viewport.Unproject(
                new Vector3( point.X, point.Y, 0 ),
                this.Projection, this.View, Matrix.Identity
            );

            return new Atom.Math.Vector2( projected.X, projected.Y );
        }

        public Atom.Math.Vector2 UnprojectIdentity( Atom.Math.Vector2 point )
        {
            var identity = Matrix.Identity;
            Vector3 projected = this.device.Viewport.Unproject(
                new Vector3( point.X, point.Y, 0 ),
                this.Projection, identity, identity
            );

            return new Atom.Math.Vector2( projected.X, projected.Y );
        }

        public Atom.Math.Vector2 Project( Atom.Math.Vector2 point )
        {
            Vector3 projected = this.device.Viewport.Project(
                new Vector3( point.X, point.Y, 0 ),
                this.Projection, this.View, Matrix.Identity
            );

            return new Atom.Math.Vector2( projected.X, projected.Y );
        }

        public void ZoomBy( float zoomChange )
        {
            float zoomTarget;

            if( !this.isZooming )
            {
                zoomTarget = this.Zoom;
            }
            else
            {
                zoomTarget = this.zoomTarget;
            }

            zoomTarget += zoomChange;

            this.ZoomTo( zoomTarget );
        }

        public void ZoomTo( float zoomTarget )
        {
            this.initialZoom = this.Zoom;
            this.zoomTarget = this.CrampZoom( zoomTarget );
            this.timeLeftToZoomTarget = ZoomTime;
            this.isZooming = true;
        }

        private float CrampZoom( float value )
        {
            return MathHelper.Max( value, MinimumZoom );
        }

        public void Update( IFlyUpdateContext updateContext )
        {
            UpdateZoom( updateContext );
        }

        private void UpdateZoom( IFlyUpdateContext updateContext )
        {
            if( !isZooming )
            {
                return;
            }

            timeLeftToZoomTarget -= updateContext.FrameTime;

            if( timeLeftToZoomTarget <= 0.0f )
            {
                this.Zoom = zoomTarget;
                timeLeftToZoomTarget = 0.0f;
                zoomTarget = 0.0f;
                isZooming = false;
            }
            else
            {
                float factor = 1.0f - (timeLeftToZoomTarget / ZoomTime);
                this.Zoom = Atom.Math.MathUtilities.Lerp( initialZoom, zoomTarget, factor );
            }
        }

        private float initialZoom;
        private float zoomTarget;
        private float timeLeftToZoomTarget = ZoomTime;
        private bool isZooming;

        private float zoom = 1.0f;
        private IFlyEntity entityToFollow;
        private GraphicsDevice device;
        private IViewSizeService viewSizeService;
        private Atom.Math.Vector2 scroll;
    }
}
