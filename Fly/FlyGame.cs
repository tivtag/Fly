// <copyright file="FlyGame.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.FlyGame class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    using Atom;
    using Atom.Fmod;
    using Atom.Math;
    using Fly.Audio;
    using Fly.Graphics;
    using Fly.States;
    using Fly.States.Editor;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Represents the <see cref="Xna.Game"/> class that is the root class
    /// of the game.
    /// </summary>
    public sealed class FlyGame : Xna.Game
    {
        public RandMT Rand
        {
            get
            {
                return this.rand;
            }
        }

        public GameGraphics Graphics
        {
            get
            {
                return this.graphics;
            }
        }

        public GameStateManager States
        {
            get
            {
                return this.gameStates;
            }
        }

        public FlyGame()
        {
            this.IsMouseVisible = true;
            this.graphics = new GameGraphics( this );
        }

        protected override void LoadContent()
        {
            this.SetupPhysics();
            this.SetupSound();
            this.SetupGraphics();
            this.SetupGameStates();
        }

        private void SetupGameStates()
        {
            var ingame = new IngameState( this );
            var editor = new EditorState( this );

            gameStates.Add( ingame );
            gameStates.Add( editor );
            gameStates.Push<EditorState>();
        }

        private void SetupSound()
        {
            this.audioSystem.Initialize( 32 );
            this.jukeBox = new JukeBox( this.rand, this.audioSystem );
            this.jukeBox.Initialize();

            this.jukeBox.ChangeTo( "lewd-day.mp3" );
        }

        private void SetupGraphics()
        {
            this.graphics.Load();
        }

        private void SetupPhysics()
        {
            FarseerPhysics.Settings.VelocityIterations = 5;
            FarseerPhysics.Settings.PositionIterations = 3;
            FarseerPhysics.Settings.EnableDiagnostics = false;
        }

        protected override void Draw( Xna.GameTime gameTime )
        {
            IFlyDrawContext drawContext = this.graphics.DrawContext;
            drawContext.GameTime = gameTime;

            gameStates.Current.Draw( drawContext );

            base.Draw( gameTime );
        }

        protected override void Update( Xna.GameTime gameTime )
        {
            updateContext.GameTime = gameTime;
            gameStates.Current.Update( updateContext );
            audioSystem.Update();
        }

        private JukeBox jukeBox;
        private readonly GameStateManager gameStates = new GameStateManager( 2 );
        private readonly AudioSystem audioSystem = new AudioSystem();

        private readonly GameGraphics graphics;
        private readonly IFlyUpdateContext updateContext = new FlyUpdateContext();
        private readonly RandMT rand = new RandMT();
    }
}