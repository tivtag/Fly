// <copyright file="BreakableCircleDescription.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Physics.Descriptors.BreakableCircleDescription class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Physics.Descriptors
{
    using System;
    using System.Collections.Generic;
    using FarseerPhysics.Dynamics;
    using Fly.Saving;

    /// <summary>
    /// Implements an IMultiPhysicsBodyDescription that builds new Circle-shaped physics objects
    /// with a specific configuration that can break into multiple parts.
    /// </summary>
    public sealed class BreakableCircleDescription : BreakablePolygonDescription, IScaleable
    {
        /// <summary>
        /// Gets or sets the radius of the Circles being build by this BreakableCircleDescription instance.
        /// </summary>
        public float Radius 
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the number of outer edges that make upthe Circles being build by this BreakableCircleDescription instance.
        /// </summary>
        public int EdgeCount 
        { 
            get; 
            set;
        }

        public override Tuple<IEnumerable<Fixture>, Body, object> Build( World world )
        {
            this.Vertices = FarseerPhysics.Common.PolygonTools.CreateCircle( this.Radius, this.EdgeCount );
            return base.Build( world );
        }
        
        public void ScaleTo( Atom.Math.Vector2 factor )
        {
            this.Radius = factor.Length * 0.05f;
        }
        
        public override void Serialize( Atom.Storage.ISerializationContext context )
        {
            base.Serialize( context );

            context.WriteDefaultHeader();
            context.Write( this.Radius );
            context.Write( this.EdgeCount );
        }

        public override void Deserialize( Atom.Storage.IDeserializationContext context )
        {
            base.Deserialize( context );

            context.ReadDefaultHeader( this.GetType() );
            this.Radius = context.ReadSingle();
            this.EdgeCount = context.ReadInt32();
        }
    }
}
