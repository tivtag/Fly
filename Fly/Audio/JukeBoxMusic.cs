// <copyright file="JukeBoxMusic.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Audio.JukeBoxMusic class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Audio
{
    /// <summary>
    /// Represents a music resource that is played in the background.
    /// </summary>
    /// <remarks>
    /// Every scene has a list of JukeBoxMusic from which is randomly choosen.
    /// </remarks>
    public sealed class JukeBoxMusic
    {
        /// <summary>
        /// Gets or sets the name that uniquely identifies this JukeBoxMusic.
        /// </summary>
        /// <remarks>
        /// This does not include the path; but does include the extension.
        /// </remarks>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the volumne this JukeBoxMusic is played back at.
        /// </summary>
        /// <value>
        /// A value between 0.0 = silent and 1.0 = full. The default value is 1.0f.
        /// </value>
        public float Volumne
        {
            get
            {
                return this.volumne;
            }

            set
            {
                this.volumne = value;
            }
        }

        /// <summary>
        /// Represents the storage field of the <see cref="Volumne"/> property.
        /// </summary>
        private float volumne = 1.0f;
    }
}
