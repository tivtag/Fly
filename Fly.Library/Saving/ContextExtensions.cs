// <copyright file="ContextExtensions.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Saving.ContextExtensions class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Saving
{
    using System;
    using Atom;
    using Atom.Storage;

    /// <summary>
    /// Defines extension methods for the <see cref="IZeldaSerializationContext"/> and
    /// <see cref="IZeldaDeserializationContext"/> interfaces.
    /// </summary>
    public static class ContextExtensions
    {
        /// <summary>
        /// Writes a version header to the ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the serialization process takes place.
        /// Provides access to required objects.
        /// </param>
        /// <param name="version">
        /// The current version of the object to serialize.
        /// </param>
        public static void WriteHeader( this ISerializationContext context, int version )
        {
            context.Write( version );
        }

        /// <summary>
        /// Writes a default version header to the ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the serialization process takes place.
        /// Provides access to required objects.
        /// </param>
        public static void WriteDefaultHeader( this ISerializationContext context )
        {
            const int Version = 1;
            context.Write( Version );
        }

        /// <summary>
        /// Reads a version header from the IDeserializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the deserialization process takes place.
        /// Provides access to required objects.
        /// </param>
        /// <param name="minimumVersion">
        /// The minimum expected version.
        /// </param>
        /// <param name="maximumVersion">
        /// The maximum expected version.
        /// </param>
        /// <param name="type">
        /// The type the header relates to.
        /// </param>
        /// <returns>
        /// The version that has been read.
        /// </returns>
        public static int ReadHeader( this IDeserializationContext context, int minimumVersion, int maximumVersion, Type type )
        {
            int version = context.ReadInt32();
            ThrowHelper.InvalidVersion( version, minimumVersion, maximumVersion, type );

            return version;
        }

        /// <summary>
        /// Reads a version header from the IDeserializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the deserialization process takes place.
        /// Provides access to required objects.
        /// </param>
        /// <param name="maximumVersion">
        /// The maximum expected version.
        /// </param>
        /// <param name="type">
        /// The type the header relates to.
        /// </param>
        /// <returns>
        /// The version that has been read.
        /// </returns>
        public static int ReadHeader( this IDeserializationContext context, int maximumVersion, Type type )
        {
            int version = context.ReadInt32();
            ThrowHelper.InvalidVersion( version, 1, maximumVersion, type );

            return version;
        }

        /// <summary>
        /// Reads a default version header from the IDeserializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the deserialization process takes place.
        /// Provides access to required objects.
        /// </param>
        /// <param name="type">
        /// The type the header relates to.
        /// </param>
        public static void ReadDefaultHeader( this IDeserializationContext context, Type type )
        {
            const int Version = 1;
            int version = context.ReadInt32();

            ThrowHelper.InvalidVersion( version, Version, type );
        }

        /// <summary>
        /// Reads a default version header to the ISerializationContext.
        /// </summary>
        /// <param name="context">
        /// The context under which the deserialization process takes place.
        /// Provides access to required objects.
        /// </param>
        /// <param name="typeName">
        /// The type name of the type the header relates to.
        /// </param>
        public static void ReadDefaultHeader( this IDeserializationContext context, string typeName )
        {
            const int Version = 1;
            int version = context.ReadInt32();

            ThrowHelper.InvalidVersion( version, Version, typeName );
        }

        /// <summary>
        /// Writes the given <see cref="Object"/>.
        /// </summary>
        /// <param name="context">
        /// The context under which the serialization process takes place.
        /// Provides access to required objects.
        /// </param>
        /// <param name="object">
        /// The object to serialize.
        /// </param>
        public static void WriteObject( this ISerializationContext context, object @object )
        {
            var obj = (ISaveable)@object;

            if( obj != null )
            {
                string typeName = obj.GetType().GetTypeName();
                context.Write( typeName );

                obj.Serialize( context );
            }
            else
            {
                context.Write( string.Empty );
            }
        }

        /// <summary>
        /// Deserializes an object of the specified base type.
        /// </summary>
        /// <typeparam name="TBase">
        /// The <see cref="ISaveable"/> type to deserialize.
        /// </typeparam>
        /// <param name="context">
        /// The context under which the deserialization process takes place.
        /// Provides access to required objects.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        public static TBase ReadObject<TBase>( this IDeserializationContext context )
            where TBase : class, ISaveable
        {
            string typeName = context.ReadString();
            if( typeName.Length == 0 )
                return null;

            Type type = Type.GetType( typeName );
            TBase obj = Activator.CreateInstance( type ) as TBase;

            if( obj != null )
            {
                obj.Deserialize( context );

                //var setupable = obj as IZeldaSetupable;

                //if( setupable != null )
                //{
                //    setupable.Setup( context.ServiceProvider );
                //}
            }

            return obj;
        }
    }
}