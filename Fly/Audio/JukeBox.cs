// <copyright file="JukeBox.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Audio.JukeBox class and Fly.Audio.BackgroundMusicMode enumeration.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Audio
{
    using System;
    using System.Diagnostics.Contracts;
    using Atom;
    using Atom.Fmod;
    using Atom.Fmod.Native;
    using Atom.Math;

    /// <summary>
    /// Manages the music that is playing in the background of the game.
    /// This class can't be inherited.
    /// </summary>
    public sealed class JukeBox
    {
        #region [ Events ]

        /// <summary>
        /// Fired when the currently playing background music has changed.
        /// </summary>
        public event Atom.RelaxedEventHandler<Channel> Changed;

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets the current mode of this JukeBox.
        /// </summary>
        /// <value>The default value is <see cref="JukeBoxMode.Random"/>.</value>
        public JukeBoxMode Mode
        {
            get
            {
                return this.mode;
            }

            set
            {
                if( value == this.Mode )
                    return;

                this.mode = value;

                switch( value )
                {
                    case JukeBoxMode.Loop:
                        this.next = this.current;
                        break;

                    case JukeBoxMode.Random:
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the fading in
        /// of music is enabled.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool FadeInIsEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the fading out
        /// of music is enabled.
        /// </summary>
        /// <value>The default value is true.</value>
        public bool FadeOutIsEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of background music that
        /// play if the JukeBox's mode is set to
        /// <see cref="JukeBoxMode.Random"/>.
        /// </summary>
        /// <value>
        /// The list of background music.
        /// </value>
        public JukeBoxMusic[] MusicList
        {
            get
            {
                return this.musicList;
            }

            set
            {
                Contract.Requires<ArgumentNullException>( value != null );

                this.musicList = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="ChannelGroup"/> all background music runs under.
        /// </summary>
        public ChannelGroup ChannelGroup
        {
            get
            {
                return this.channelGroup;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether changing the background music
        /// using <see cref="ChangeTo(String)"/> is currently allowed.
        /// </summary>
        public bool ManualChangeAllowed
        {
            get;
            set;
        }

        #endregion

        #region [ Initialization ]

        /// <summary>
        /// Initializes a new instance of the <see cref="JukeBox"/> class.
        /// </summary>
        /// <param name="rand">
        /// A random number generator.
        /// </param>
        /// <param name="audioSystem">
        /// The audo system responsible
        /// </param>
        public JukeBox( IRand rand, AudioSystem audioSystem )
        {
            this.rand        = rand;
            this.audioSystem = audioSystem;

            this.ManualChangeAllowed = true;
            this.FadeInIsEnabled  = true;
            this.FadeOutIsEnabled = true;
        }

        /// <summary>
        /// Initializes this JukeBox.
        /// </summary>
        public void Initialize()
        {
            this.channelGroup = new ChannelGroup( "Background Music", audioSystem );
            this.audioSystem.MasterChannelGroup.AddChildGroup( this.channelGroup );
        }

        #endregion

        #region [ Methods ]

        #region > Update <

        /// <summary>
        /// Updates this JukeBox.
        /// </summary>
        /// <param name="updateContext">
        /// The current ZeldaUpdateContext.
        /// </param>
        public void Update( IUpdateContext updateContext )
        {
            if( currentChannel == null )
                return;

            if( this.isStoppingManually )
            {
                if( FadeOutIsEnabled )
                {
                    this.fadeOutTimeLeft -= updateContext.FrameTime;

                    if( this.fadeOutTimeLeft <= 0.0f )
                    {
                        this.currentChannel.Stop();
                        this.isStoppingManually = false;
                    }
                    else
                    {
                        this.currentChannel.Volume = fadeOutTimeLeft / fadeOutTime;
                    }
                }
                else
                {
                    this.currentChannel.Stop();
                    this.isStoppingManually = false;
                }
            }
            else
            {
                uint position = currentChannel.GetPosition( TIMEUNIT.MS );
                uint length   = current.GetLength( TIMEUNIT.MS );

                float time    = (float)position / 1000.0f;
                float timeEnd = (float)length / 1000.0f;

                // Fade In:
                if( this.FadeInIsEnabled && time <= fadeInTime )
                {
                    float volumne = time / fadeInTime;

                    // To fight rounding errors:
                    if( volumne >= 0.99f )
                        volumne = 1.0f;
                }
                else if( this.FadeOutIsEnabled && (time >= timeEnd - fadeOutTime) )
                {
                    // Fade Out:
                    this.currentChannel.Volume = (timeEnd - time) / fadeOutTime;
                }
            }
        }

        #endregion

        #region > Change <

        /// <summary>
        /// Tells this JukeBox to change
        /// to the background music with the given name.
        /// </summary>
        /// <param name="musicName">
        /// The name that uniquely identifies the
        /// music resource to change to.
        /// </param>
        public void ChangeTo( string musicName )
        {
            var music = this.audioSystem.GetMusic( musicName );
            if( music == null )
                return;

            music.LoadAsMusic( false );
            this.ChangeTo( music );
        }

        /// <summary>
        /// Tells this JukeBox to change 
        /// to the given background <paramref name="music"/>.
        /// </summary>
        /// <param name="music">
        /// The music to change to. (Must be loaden!)
        /// </param>
        public void ChangeTo( Sound music )
        {
            Contract.Requires<ArgumentNullException>( music != null );

            if( !this.ManualChangeAllowed )
            {
                this.next = music;
                return;
            }

            this.ChangeToImpl( music );
        }

        /// <summary>
        /// Tells this JukeBox to change 
        /// to the given background <paramref name="music"/>.
        /// </summary>
        /// <param name="music">
        /// The music to change to. (Must be loaden!)
        /// </param>
        private void ChangeToImpl( Sound music )
        {
            if( this.current == null || !this.currentChannel.IsPlaying )
            {
                // Unhook old.
                if( this.current != null )
                {
                    this.currentChannel.Ended -= this.OnCurrentChannel_Ended;
                }

                // Hook new.
                this.current = music;
                this.currentChannel = music.Play();
                this.currentChannel.Priority = 0;
                this.currentChannel.Ended += this.OnCurrentChannel_Ended;
                this.currentChannel.ChannelGroup = this.channelGroup;

                this.FindNext();
                this.OnChanged();
            }
            else
            {
                this.next = music;

                if( !isStoppingManually )
                {
                    this.fadeOutTimeLeft    = fadeOutTime;
                    this.isStoppingManually = true;
                }
            }
        }

        /// <summary>
        /// Fires the <see cref="Changed"/> event.
        /// </summary>
        private void OnChanged()
        {
            if( this.Changed != null )
            {
                this.Changed( this, this.currentChannel );
            }
        }

        /// <summary>
        /// Tells this JukeBox to randomly change 
        /// to one of the background music in the <see cref="MusicList"/>.
        /// </summary>
        public void ChangeToRandom()
        {
            var music = this.SelectRandomMusic();

            if( music != null && music != this.current )
            {
                this.ChangeTo( music );
            }
        }

        #endregion

        #region > Volume <

        /// <summary>
        /// Changes the volume of the background music to the given value.
        /// </summary>
        /// <param name="newVolume">
        /// The volume (a value between 0 and 1) to change to.
        /// </param>
        internal void ChangeVolumeTo( float newVolume )
        {
            this.oldVolume = this.channelGroup.Volume;
            this.channelGroup.Volume = newVolume;
        }

        /// <summary>
        /// Restores the volume of the background music to the value
        /// before the last call to <see cref="ChangeVolumeTo"/>.
        /// </summary>
        internal void RestoreVolume()
        {
            this.channelGroup.Volume = oldVolume;
        }

        #endregion

        /// <summary>
        /// Gets a value indicating whether the music with the given name
        /// is currently playing in the background.
        /// </summary>
        /// <param name="musicName">
        /// The name that uniquely identifies the music.
        /// </param>
        /// <returns>
        /// Returns true if the music with the given name is currently playing;
        /// otherwise false.
        /// </returns>
        internal bool IsPlaying( string musicName )
        {
            if( this.current != null && this.current.Name.Equals( musicName, StringComparison.OrdinalIgnoreCase ) )
                return true;

            if( this.isStoppingManually )
            {
                if( this.next != null && this.next.Name.Equals( musicName, StringComparison.OrdinalIgnoreCase ) )
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Finds the next music to play, depending on the current BackgroundMusicMode.
        /// </summary>
        private void FindNext()
        {
            switch( this.mode )
            {
                case JukeBoxMode.Random:
                    this.next = this.SelectRandomMusic();
                    break;

                case JukeBoxMode.Loop:
                    this.next = current;
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Randomly selects a music from the music list.
        /// </summary>
        /// <returns>
        /// The music that has been selected.
        /// </returns>
        private Sound SelectRandomMusic()
        {
            JukeBoxMusic music = this.ActuallySelectRandomMusic();
            if( music == null )
                return null;

            Sound musicResource = audioSystem.GetMusic( music.FileName );

            if( musicResource != null )
            {
                musicResource.LoadAsMusic( false );
            }

            return musicResource;
        }

        /// <summary>
        /// Randomly selects a <see cref="JukeBoxMusic"/> from the musicList.
        /// </summary>
        /// <param name="recursionDepth">
        /// The number of times ActuallySelectRandomMusic has been called recursively;
        /// this happens when the IRequirement of a choosen JukeBoxMusic has not been fulfilled.
        /// </param>
        /// <returns>
        /// The JukeBoxMusic that has been selected;
        /// or null if none.
        /// </returns>
        private JukeBoxMusic ActuallySelectRandomMusic( int recursionDepth = 0 )
        {
            const int MaximumRecursionDepth = 10;
            if( this.musicList.Length == 0 )
                return null;

            if( recursionDepth >= MaximumRecursionDepth )
            {
                return this.musicList[0];
            }

            JukeBoxMusic music = this.musicList[rand.RandomRange( 0, musicList.Length - 1 )];
            if( music == null )
                return null;
                        
            if( !this.CanPlayMusic( music )  )
            {
                return this.ActuallySelectRandomMusic( ++recursionDepth );    
            }

            return music;
        }

        private bool CanPlayMusic( JukeBoxMusic music )
        {
            //            if( music.Requirement != null )
            //{
            //    if( !music.Requirement.IsFulfilledBy( this.ingameState.Player ) )
            return true;
        }

        /// <summary>
        /// Called when the current song has ended.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        private void OnCurrentChannel_Ended( Channel sender )
        {
            if( this.next == null )
                this.FindNext();

            if( this.next != null )
                this.ChangeToImpl( this.next );
        }

        /// <summary>
        /// Stops playing the current background music,
        /// without starting a new one.
        /// </summary>
        public void Stop()
        {
            if( this.currentChannel != null )
            {
                this.currentChannel.Ended -= this.OnCurrentChannel_Ended;
                this.currentChannel.Stop();

                this.currentChannel = null;
            }

            this.current = null;
            this.fadeOutTimeLeft = 0.0f;
            this.isStoppingManually = false;
        }

        #endregion

        #region [ Fields ]

        /// <summary>
        /// Identifies the currently playing background music.
        /// </summary>
        private Sound current;

        /// <summary>
        /// Identifies the channel the current background music is playing on.
        /// </summary>
        private Channel currentChannel;

        /// <summary>
        /// The next music to play.
        /// </summary>
        private Sound next;

        /// <summary>
        /// The current BackgroundMusicMode.
        /// </summary>
        private JukeBoxMode mode;

        /// <summary>
        /// The list of random songs.
        /// </summary>
        private JukeBoxMusic[] musicList = new JukeBoxMusic[0];

        /// <summary>
        /// States whether the current music is currently stopping to play.
        /// </summary>
        private bool isStoppingManually;

        /// <summary>
        /// The duration the music fades in/out for.
        /// </summary>
        private const float fadeInTime = 2.5f, fadeOutTime = 3.0f;

        /// <summary>
        /// The time that has passed since the fide in/out command.
        /// </summary>
        private float fadeOutTimeLeft;

        /// <summary>
        /// Stores the volume of the background music before the last call to <see cref="ChangeVolumeTo"/>.
        /// </summary>
        private float oldVolume = 1.0f;

        /// <summary>
        /// Idenfities the ChannelGroup under which the background music is grouped. 
        /// </summary>
        private ChannelGroup channelGroup;
        
        /// <summary>
        /// A random number generator.
        /// </summary>
        private readonly IRand rand;

        /// <summary>
        /// The Atom.Fmod.AudioSystem object.
        /// </summary>
        private readonly AudioSystem audioSystem;

        #endregion
    }

    /// <summary>
    /// Enumerates the different modes the <see cref="JukeBox"/> supports.
    /// </summary>
    public enum JukeBoxMode
    {
        /// <summary>
        /// The next song is randomly selected from a list of songs.
        /// </summary>
        Random = 0,

        /// <summary>
        /// The current song is looping until the mode is changed.
        /// </summary>
        Loop
    }
}
