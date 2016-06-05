// <copyright file="TriangleDescription.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Physics.Descriptors.TriangleDescription class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Physics.Descriptors
{
    using FarseerPhysics.Common;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Implements an IPhysicsBodyDescription that builds new same-sided Triangle-shaped physics objects
    /// with a specific configuration.
    /// </summary>
    public sealed class TriangleDescription : BasePhysicsBodyDescription
    {
        /// <summary>
        /// Gets the size of the side of the Triangles build with this TriangleDescription instance.
        /// </summary>
        public float SideSize { get; set; }

        public override Body Build( World world )
        {
            Vector2 a = new Vector2();
            Vector2 b = new Vector2( SideSize, 0.0f );
            Vector2 c = new Vector2( SideSize/2.0f, SideSize/2.0f );
            var verts = new Vertices( new Vector2[] { a, b, c } );
            
            var triangle = BodyFactory.CreatePolygon( world, verts, this.Density );
            return BaseInit( triangle );
        }
    }
}
