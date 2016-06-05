// <copyright file="PolygonDescription.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Physics.Descriptors.PolygonDescription class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Physics.Descriptors
{
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;

    /// <summary>
    /// Implements an IPhysicsBodyDescription that builds new Polygon-shaped physics objects
    /// with a specific configuration.
    /// </summary>
    public sealed class PolygonDescription : BasePhysicsBodyDescription
    {
        public FarseerPhysics.Common.Vertices Vertices { get; set; }

        public override Body Build( World world )
        {
            var polygon = BodyFactory.CreatePolygon( world, this.Vertices, this.Density );
            return this.BaseInit( polygon );
        }
    }
}
