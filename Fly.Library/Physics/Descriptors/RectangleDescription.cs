// <copyright file="RectangleDescription.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Physics.Descriptors.RectangleDescription class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Physics.Descriptors
{
    using System;
    using Atom.Storage;
    using FarseerPhysics.Dynamics;
    using FarseerPhysics.Factories;
    using Fly.Saving;

    /// <summary>
    /// Implements an IPhysicsBodyDescription that builds new Rectangle-shaped physics objects
    /// with a specific configuration.
    /// </summary>
    public sealed class RectangleDescription : BasePhysicsBodyDescription, IScaleable
    {
        /// <summary>
        /// Gets or sets the width of the Rectangles build with this RectangleDescription instance.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the Rectangles build with this RectangleDescription instance.
        /// </summary>
        public float Height { get; set; }

        public override Body Build( World world )
        {
            var rectangle = BodyFactory.CreateRectangle( world, this.Width, this.Height, this.Density );
            return this.BaseInit( rectangle );
        }

        public void ScaleTo( Atom.Math.Vector2 factor )
        {
            this.Width = Atom.Math.MathUtilities.Clamp( Math.Abs( factor.X ), 1, 1000 );
            this.Height = Atom.Math.MathUtilities.Clamp( Math.Abs( factor.Y ), 1, 1000 );
        }

        public override void Serialize( ISerializationContext context )
        {
            context.WriteDefaultHeader();

            context.Write( this.IsStatic );
            context.Write( this.Density );
            context.Write( this.Width );
            context.Write( this.Height );
        }

        public override void Deserialize( IDeserializationContext context )
        {
            context.ReadDefaultHeader( this.GetType() );

            this.IsStatic = context.ReadBoolean();
            this.Density = context.ReadSingle();
            this.Width = context.ReadSingle();
            this.Height = context.ReadSingle();
        }
    }
}
