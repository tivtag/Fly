// <copyright file="IRenderPipeline.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.IRenderPipeline interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics
{
    using System;

    /// <summary>
    /// A rendering pipeline is responsible for outputting the drawn objects onto the screen.
    /// </summary>
    public interface IRenderPipeline : IFlyDrawable
    {
        /// <summary>
        /// Gets or sets the action that does the rendering of all objects, entities and shapes using the given IFlyDrawContext.
        /// </summary>
        Action<IFlyDrawContext> RenderAction 
        {
            get; 
            set;
        }

        void Load();
    }
}
