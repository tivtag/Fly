// <copyright file="IngameState.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.States.IngameState class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.States
{
    using Atom;
    using Atom.Math;
    using Fly.Entities;
    using Fly.Entities.Concrete;
    using Fly.Graphics;
    using Fly.Saving;
    using Fly.UI;
    using Fly.Waves;
    using Microsoft.Xna.Framework.Input;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// The game state in which the player plays the game.
    /// </summary>
    public sealed class IngameState : IGameState
    {
        public IngameState( FlyGame game )
        {
            this.rand = game.Rand;
            this.graphics = game.Graphics;
            this.game = game;
        }

        public void Load()
        {
            LoadWorldScene();
            LoadPlayer();
            LoadPlayerTurret();
            LoadHomePlanet();
            LoadMoon();
            LoadCamera();
            LoadUserInterface();
            LoadIngameRenderer();

            this.OnSceneLoaded( scene );
        }

        private void LoadWorldScene()
        {
            SceneStorage sceneLoader = new SceneStorage();
            this.world = GameWorld.Create( sceneLoader.Load(), rand );
            this.scene = world.Scene;
        }

        private void LoadPlayer()
        {
            this.player = EntityFactory.CreateRectangularShip( 0.5f, 1.0f, 4.0f, this.world.PlayerEmpire );
            this.player.DrawStrategy = new Fly.Graphics.Strategies.Ships.TierAShipDrawStrategy();
            this.player.Name = "Ship";
            this.player.Position = new Vector2( 5, 5 );
            this.world.AddEntity( player );

            this.player.Physics.AngularDamping = 12.0f;
            this.player.Physics.LinearDamping = 0.05f; // 0.1025f;

            world.Player = player;
        }

        private void LoadPlayerTurret()
        {
            turrent = EntityFactory.CreateTurrent( player );
            world.AddEntity( turrent );
        }

        private void LoadHomePlanet()
        {
            HomePlanet homePlanet = EntityFactory.CreateHomePlanet( player.OwnedBy.Empire, 30.0f, 1000.0f, false, 64 );
            homePlanet.Name = "Home Planet";
            homePlanet.Position = new Vector2( 50, 80 );

            world.PlayerEmpire.HomePlanet = homePlanet;
        }

        private void LoadMoon()
        {
            var moon = EntityFactory.CreatePhysicsCircle( 8.0f, 50.0f );
            moon.Name = "Moon";
            moon.DrawStrategy.Color = Xna.Color.DimGray;
            moon.Position = new Vector2( 15, 10 );
            this.world.AddEntity( moon );

            moon.Physics.LinearVelocity = new Vector2( -3.0f, 6.0f );
        }

        private void LoadCamera()
        {
            this.camera = new Camera( graphics.ViewSizeService, graphics.Device );
            this.camera.EntityToFollow = this.player;
            this.graphics.DrawContext.Camera = this.camera;

            this.world.Camera = camera;
        }

        private void LoadUserInterface()
        {
            this.userInterface = new IngameUserInterface();
            this.userInterface.KeyboardInput += this.OnKeyboardInput;
            this.userInterface.MouseInput += this.OnMouseInput;
            this.userInterface.Setup( this.player );
            this.userInterface.TrackEntity( this.player );
            this.userInterface.AddElement( new SelectionInfoDisplay( this.selectionContainer ) );
        }

        private void LoadIngameRenderer()
        {
            ingameRenderer = new IngameRenderer( this.graphics.DeviceService, this.graphics.Pipeline );
            ingameRenderer.Scene = this.world.Scene;
            ingameRenderer.UserInterface = this.userInterface;
            ingameRenderer.Load();
        }

        private void OnSceneLoaded( FlyScene scene )
        {
            var asteroidPusher = new AsteroidPusher( rand );
            asteroidPusher.Push( scene );
        }

        public void Unload()
        {
        }

        public void Draw( IDrawContext drawContext )
        {
            var flyContext = (IFlyDrawContext)drawContext;
            this.ingameRenderer.Draw( flyContext );
        }

        public void Update( IUpdateContext updateContext )
        {
            this.updateContext = (IFlyUpdateContext)updateContext;

            camera.Update( this.updateContext );
            world.Update( this.updateContext );
            userInterface.Update( updateContext );
            
            player.Physics.LimitVelocityTo( 38.0f );
        }
        
        private void OnKeyboardInput( object sender, ref KeyboardState keyState, ref KeyboardState oldKeyState )
        {
            Vector2 force = Vector2.Zero;

            if( keyState.IsKeyDown( Keys.Escape ) && oldKeyState.IsKeyUp( Keys.Escape ) )
            {
                game.Exit();
            }

            if( keyState.IsKeyDown( Keys.Space ) && oldKeyState.IsKeyUp( Keys.Space ) )
            {
                this.Shoot();
            }

            if( keyState.IsKeyDown( Keys.LeftControl ) && oldKeyState.IsKeyUp( Keys.LeftControl ) )
            {
                this.Shoot( 100000.0f );
            }

            if( keyState.IsKeyDown( Keys.LeftAlt ) && oldKeyState.IsKeyUp( Keys.LeftAlt ) )
            {
                this.ShootRocket();
            }

            if( keyState.IsKeyDown( Keys.X ) && oldKeyState.IsKeyUp( Keys.X ) )
            {
                this.ShootBlackHole();
            }

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

            if( keyState.IsKeyDown( Keys.F5 ) && oldKeyState.IsKeyUp( Keys.F5 ) )
            {
                camera.ZoomTo( 30.0f );
            }

            if( keyState.IsKeyDown( Keys.B ) && oldKeyState.IsKeyUp( Keys.B ) )
            {
                CreateTractorBeam();
            }

            const float TorqueAmount = 24;
            float forceAmount = 30;
            float torque = 0;
            const float SpeedBoost = 1.7f;
            
            // Speed
            if( keyState.IsKeyDown( Keys.LeftShift ) || keyState.IsKeyDown( Keys.RightShift ) )
            {
                forceAmount *= SpeedBoost;
            }

            // Force Stop
            if( keyState.IsKeyDown( Keys.R ) )
            {
                this.player.Physics.LinearVelocity -= (this.player.Physics.LinearVelocity * (updateContext.FrameTime * 0.85f));
            }

            // Rotate
            if( keyState.IsKeyDown( Keys.A ) )
            {
                torque += TorqueAmount;
            }

            if( keyState.IsKeyDown( Keys.D ) )
            {
                torque -= TorqueAmount;
            }

            // Accelerate
            if( keyState.IsKeyDown( Keys.W ) )
            {
                force += Vector2.FromAngle( player.Rotation ) * forceAmount;
            }

            if( keyState.IsKeyDown( Keys.S ) )
            {
                force -= Vector2.FromAngle( player.Rotation ) * 10.0f;
            }

            if( keyState.IsKeyDown( Keys.Q ) )
            {
                force += Vector2.FromAngle( player.Rotation + Atom.Math.Constants.PiOver2 ) * forceAmount * 0.8f;
            }

            if( keyState.IsKeyDown( Keys.E ) )
            {
                force -= Vector2.FromAngle( player.Rotation + Atom.Math.Constants.PiOver2 ) * forceAmount * 0.8f;
            }

            player.Physics.ApplyForce( force );
            player.Physics.ApplyTorque( torque );

            if( keyState.IsKeyDown( Keys.Up ) )
            {
                camera.Zoom += 0.01f;
            }
            if( keyState.IsKeyDown( Keys.Down ) )
            {
                camera.Zoom -= 0.01f;
            }

            if( keyState.IsKeyDown( Keys.F10 ) )
            {
                game.States.Replace<Fly.States.Editor.EditorState>();
            }
        }

        private void CreateTractorBeam()
        {
            if( this.beam != null )
            {
                this.world.RemoveEntity( beam );
                beam = null;
            }

            if( !this.CreateTractorBeamTo( this.selectionContainer.HoveredEntity ) )
            {
                this.CreateTractorBeamTo( this.selectionContainer.SelectedEntity );
            }
        }

        private bool CreateTractorBeamTo( IPhysicsEntity entity )
        {
            if( entity != null && !entity.Physics.IsStatic )
            {
                this.beam = EntityFactory.CreateTractorBeam( this.player, entity );

                if( beam != null )
                {
                    this.world.AddEntity( beam );
                    return true;
                }
            }

            return false;
        }

        private void OnMouseInput( object sender, ref MouseState mouseState, ref MouseState oldMouseState )
        {
            Vector2 worldPosition = camera.Unproject( new Vector2( mouseState.X, mouseState.Y ) );
            this.selectionContainer.Hover( this.scene.GetPhysicsEntityAt( worldPosition ) );

            if( mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released )
            {
                this.selectionContainer.SelectHovered();
            }

            if( mouseState.ScrollWheelValue != oldMouseState.ScrollWheelValue )
            {
                int deltaScroll = oldMouseState.ScrollWheelValue - mouseState.ScrollWheelValue;
                float zoomChange = 0.005f * deltaScroll;
                camera.ZoomBy( zoomChange );
            }
        }

        private void ShootRocket()
        {
            this.turrent.BulletFactory = new RocketBulletFactory();
            this.turrent.Shoot();
        }

        private void ShootBlackHole()
        {
            this.turrent.BulletFactory = new BlackHoleBulletFactory();
            this.turrent.Shoot();
        }

        private void Shoot( float density = 100.0f )
        {
            this.turrent.BulletFactory = new BulletFactory() { BulletDensity = density };
            this.turrent.Shoot();
        }

        private void OnWaveLaunched( object sender, IWave wave )
        {
            this.userInterface.ShowInfo( "Warning! Enemy Wave incomming!", wave.ContentDescription );
        }

        public void ChangedFrom( IGameState oldState )
        {
            Load();
        }

        public void ChangedTo( IGameState newState )
        {
        }

        private Turrent turrent;
        private Ship player;
        private TractorBeam beam;
        
        private GameWorld world;
        private FlyScene scene;
        private IngameUserInterface userInterface;

        private RandMT rand;
        private GameGraphics graphics;
        private Camera camera;
        private IngameRenderer ingameRenderer;
        private FlyGame game;
        private IFlyUpdateContext updateContext;

        private readonly EntitySelectionContainer selectionContainer = new EntitySelectionContainer();
    }
}
