// <copyright file="CircleDescription.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Physics.Descriptors.CircleDescription class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Physics.Descriptors
{
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;

    /// <summary>
    /// Implements an IPhysicsBodyDescription that builds new Circle-shaped physics objects
    /// with a specific configuration.
    /// </summary>
    public sealed class CircleDescription : BasePhysicsBodyDescription
    {
        public float Radius { get; set; }

        public Category CollisionCategory { get; set; } = Category.All;

        public override Body Build( World world )
        {
            var circle = BodyFactory.CreateCircle( world, this.Radius, this.Density );
            circle.CollidesWith = this.CollisionCategory;

            return this.BaseInit( circle );
        }
    }
}
