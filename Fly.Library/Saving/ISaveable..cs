// <copyright file="ISaveable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Saving.ISaveable interface.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Saving
{
    using Atom.Storage;

    /// <summary>
    /// Provides a mechanism to read and write an object into a binary stream.
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// Serializes the data required to descripe this ISaveable.
        /// </summary>
        /// <param name="context">
        /// The context under which the serialization process takes place.
        /// Provides access to required objects.
        /// </param>
        void Serialize( ISerializationContext context );

        /// <summary>
        /// Deserializes the data required to descripe this ISaveable.
        /// </summary>
        /// <param name="context">
        /// The context under which the deserialization process takes place.
        /// Provides access to required objects.
        /// </param>
        void Deserialize( IDeserializationContext context );
    }
}
