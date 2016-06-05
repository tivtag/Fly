// <copyright file="BaseMultiPhysicsBodyDescription.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Physics.Descriptors.BaseMultiPhysicsBodyDescription class.
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
    /// Implements a basic implementation of the <see cref="IMultiPhysicsBodyDescription"/> interface
    /// that adds basic physics properties to the description.
    /// </summary>
    public abstract class BaseMultiPhysicsBodyDescription : IMultiPhysicsBodyDescription
    {
        public bool IsStatic { get; set; }

        public float Density { get; set; }

        public abstract Tuple<IEnumerable<Fixture>, Body, object> Build( World world );
        
        protected virtual void InitBody( Body body )
        {
            if( !this.IsStatic )
            {
                body.BodyType = BodyType.Dynamic;
            }

            body.IsStatic = this.IsStatic;
        }

        public virtual void Serialize( Atom.Storage.ISerializationContext context )
        {
            context.WriteDefaultHeader();

            context.Write( this.IsStatic );
            context.Write( this.Density );
        }

        public virtual void Deserialize( Atom.Storage.IDeserializationContext context )
        {
            context.ReadDefaultHeader( this.GetType() );

            this.IsStatic = context.ReadBoolean();
            this.Density = context.ReadSingle();
        }
    }
}
