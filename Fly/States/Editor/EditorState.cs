// <copyright file="EditorState.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.States.Editor.EditorState class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.States.Editor
{
    using System;
    using Atom;
    using Atom.Math;
    using Atom.Xna;
    using Fly.Entities;
    using Fly.Graphics;
    using Fly.Saving;
    using Microsoft.Xna.Framework.Input;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// The game state in which the player can edit and save the game map.
    /// </summary>
    public sealed class EditorState : IGameState
    {
        private Vector2 WorldMousePosition
        {
            get
            {
                return camera.Unproject( new Vector2( this.mouseState.X, this.mouseState.Y ) );
            }
        }

        public EditorState( FlyGame game )
        {
            this.rand = game.Rand;
            this.graphics = game.Graphics;
            this.game = game;
        }

        public void Load()
        {
            this.scene = new FlyScene( new Point2( 1000, 1000 ) );
            this.camera = new Camera( graphics.ViewSizeService, graphics.Device );
    
            this.userInterface = new EditorUserInterface();
            this.userInterface.KeyboardInput += this.OnKeyboardInput;
            this.userInterface.MouseInput += this.OnMouseInput;

            ingameRenderer = new IngameRenderer( this.graphics.DeviceService, this.graphics.Pipeline );
            ingameRenderer.Scene = this.scene;
            ingameRenderer.UserInterface = this.userInterface;
            ingameRenderer.Load();
        }

        public void Unload()
        {
        }

        public void Draw( IDrawContext drawContext )
        {
            var flyContext = (IFlyDrawContext)drawContext;
            flyContext.Camera = this.camera;

            this.ingameRenderer.Draw( flyContext );
        }

        private void DrawSelectionHelpers( IFlyDrawContext flyContext )
        {
            var entity = this.selectionContainer.SelectedEntity;

            if( entity != null )
            {
                flyContext.ShapeRenderer.DrawSolidSegment(
                    entity.Transform.Position.ToXna(),
                    (entity.Transform.Position + Vector2.FromAngle( entity.Rotation ) * 10.0f).ToXna(),
                    Xna.Color.Gray,
                    0.05f
                );
            }
        }

        public void Update( IUpdateContext updateContext )
        {
            this.updateContext = (IFlyUpdateContext)updateContext;

            scene.Update( updateContext );
            camera.Update( this.updateContext );
            userInterface.Update( updateContext );
        }

        private void OnKeyboardInput( object sender, ref KeyboardState keyState, ref KeyboardState oldKeyState )
        {
            this.keyState = keyState;

            float frameTime = updateContext.FrameTime;

            float scrollSpeed = 250.0f;

            if( keyState.IsKeyDown( Keys.W ) )
            {
                camera.Scroll += new Vector2( 0.0f, scrollSpeed ) * frameTime;
            }

            if( keyState.IsKeyDown( Keys.S ) )
            {
                camera.Scroll -= new Vector2( 0.0f, scrollSpeed ) * frameTime;
            }

            if( keyState.IsKeyDown( Keys.A ) )
            {
                camera.Scroll -= new Vector2( scrollSpeed, 0.0f ) * frameTime;
            }

            if( keyState.IsKeyDown( Keys.D ) )
            {
                camera.Scroll += new Vector2( scrollSpeed, 0.0f ) * frameTime;
            }

            // Zoom
            if( keyState.IsKeyDown( Keys.F1 ) && oldKeyState.IsKeyUp( Keys.F1 ) )
            {
                camera.ZoomTo( 1.0f );
            }

            if( keyState.IsKeyDown( Keys.F2 ) && oldKeyState.IsKeyUp( Keys.F2 ) )
            {
                camera.ZoomTo( 2.0f );
            }

            if( keyState.IsKeyDown( Keys.F3 ) && oldKeyState.IsKeyUp( Keys.F3 ) )
            {
                camera.ZoomTo( 4.0f );
            }

            if( keyState.IsKeyDown( Keys.F4 ) && oldKeyState.IsKeyUp( Keys.F4 ) )
            {
                camera.ZoomTo( 10.0f );
            }

            if( keyState.IsKeyDown( Keys.Up ) )
            {
                camera.Zoom += 0.01f;
            }
            if( keyState.IsKeyDown( Keys.Down ) )
            {
                camera.Zoom -= 0.01f;
            }

            // App control
            if( keyState.IsKeyDown( Keys.Escape ) && oldKeyState.IsKeyUp( Keys.Escape ) )
            {
                game.Exit();
            }

            if( keyState.IsKeyDown( Keys.F5 ) && oldKeyState.IsKeyUp( Keys.F5 ) )
            {
                if( SaveScene() )
                {
                    userInterface.ShowInfo( "Success!", "Map was successfully saved." );
                }
            }

            if( keyState.IsKeyDown( Keys.F6 ) && oldKeyState.IsKeyUp( Keys.F6 ) )
            {
                LoadScene();
            }
            
            if( keyState.IsKeyDown( Keys.Back ) || keyState.IsKeyDown( Keys.Delete ) )
            {
                DeleteSelected();
            }

            if( keyState.IsKeyDown( Keys.F11 ) )
            {
                game.States.Replace<IngameState>();
            }
        }

        private void DeleteSelected()
        {
            var entity = this.selectionContainer.Unselect();

            if( entity != null )
            {
                this.scene.RemoveEntity( entity );                
            }
        }

        private void LoadScene()
        {
            selectionContainer.Unselect();

            var storage = new SceneStorage();
            this.scene = storage.Load();

            if( scene != null )
            {
                scene.PhysicsWorld.ContactManager.ContactFilter = ( a, b ) => false;
            }

            ingameRenderer.Scene = scene;
        }

        private bool SaveScene()
        {
            var storage = new SceneStorage();
            return storage.Save( scene );
        }

        private void OnMouseInput( object sender, ref MouseState mouseState, ref MouseState oldMouseState )
        {
            this.mouseState = mouseState;
            this.selectionContainer.Hover( this.scene.GetPhysicsEntityAt( WorldMousePosition ) );

            // Drag and drop
            if( mouseState.LeftButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Pressed )
            {
                if( oldMouseState.LeftButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Released )
                {
                    HandleLeftRightClick( ref mouseState );
                }
                else
                {
                    HandleLeftRightMouseDrag( ref mouseState );
                }

                return;
            }

            // Avoid triggering left/right clicks after drag and drop finished.
            if( wasLeftRightClicking )
            {
                if( mouseState.LeftButton == ButtonState.Released && mouseState.RightButton == ButtonState.Released )
                {
                    wasLeftRightClicking = false;
                }

                return;
            }

            // Select, create or move object
            if( mouseState.LeftButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released )
            {
                if( oldMouseState.LeftButton == ButtonState.Released )
                {
                    HandleLeftClick( ref mouseState );
                }
                else
                {
                    HandleLeftMouseDrag( ref mouseState );
                }
            }

            // Rotate object
            else if( mouseState.RightButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released )
            {
                if( oldMouseState.RightButton == ButtonState.Released )
                {
                    HandleRightClick( ref mouseState );
                }
                else
                {
                    HandleRightMouseDrag( ref mouseState );
                }
            }

            if( mouseState.ScrollWheelValue != oldMouseState.ScrollWheelValue )
            {
                int deltaScroll = oldMouseState.ScrollWheelValue - mouseState.ScrollWheelValue;
                float zoomChange = 0.007f * deltaScroll;
                camera.ZoomBy( zoomChange );
            }
        }

        private void HandleLeftRightMouseDrag( ref MouseState mouseState )
        {
            IPhysicsEntity entity = selectionContainer.SelectedEntity;

            if( entity != null )
            {
                var scaleable = entity.Physics as IScaleable;

                if( scaleable != null )
                {
                    Vector2 delta = entity.Position - WorldMousePosition;
                    delta *= 2.0f;

                    scaleable.ScaleTo( delta );
                }
            }

            this.wasLeftRightClicking = true;
        }

        private void HandleLeftRightClick( ref MouseState mouseState )
        {
        }

        private void HandleRightClick( ref MouseState mouseState )
        {
        }

        private void HandleRightMouseDrag( ref MouseState mouseState )
        {
            IPhysicsEntity entity = selectionContainer.SelectedEntity;

            if( entity != null )
            {
                float angle = (float)Math.Atan2( WorldMousePosition.Y - entity.Position.Y, WorldMousePosition.X - entity.Position.X );

                if( keyState.IsKeyDown( Keys.LeftShift ) )
                {
                    angle = MathUtilities.RoundToMultiple( angle, Constants.PiOver4 );
                }

                entity.Rotation = angle;
            }
        }

        private void HandleLeftMouseDrag( ref MouseState mouseState )
        {
            IPhysicsEntity entity = selectionContainer.SelectedEntity;

            if( entity != null )
            {
                entity.Position = WorldMousePosition - selectionOffset;
            }
        }

        private void HandleLeftClick( ref MouseState mouseState )
        {
            // Select existing
            if( this.selectionContainer.SelectHovered() )
            {
                this.selectionOffset = WorldMousePosition - this.selectionContainer.SelectedEntity.Position;
                return;
            }
            else
            {
                this.selectionOffset = Vector2.Zero;
            }

            CreateNewEntity();
        }

        private void CreateNewEntity()
        {
            // Create new
            FlyEntity entity = EntityFactory.CreateAsteroid( 2.0f, 1.5f );  //EntityFactory.CreateWall( 10, 10 );
            entity.Position = WorldMousePosition;
            scene.AddEntity( entity );

            this.selectionContainer.Select( entity as IPhysicsEntity );
        }

        public void ChangedFrom( IGameState oldState )
        {
            Load();
            LoadScene();
        }
        
        public void ChangedTo( IGameState newState )
        {
        }

        private FlyScene scene;

        private RandMT rand;
        private GameGraphics graphics;
        private Camera camera;
        private IngameRenderer ingameRenderer;
        private FlyGame game;
        private IFlyUpdateContext updateContext;

        private EditorUserInterface userInterface;
        private MouseState mouseState;
        private bool wasLeftRightClicking;
        private Vector2 selectionOffset;

        private readonly EntitySelectionContainer selectionContainer = new EntitySelectionContainer() {
            DeselectOnDoubleSelection = false
        };
        private KeyboardState keyState;
    }
}
