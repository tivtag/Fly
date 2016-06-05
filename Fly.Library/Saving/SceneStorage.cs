// <copyright file="SceneStorage.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Saving.SceneStorage class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Saving
{
    using Atom.Math;
    using Atom.Storage;
    using Fly.Entities;
    using System;
    using System.IO;

    /// <summary>
    /// Responsible for saving/loading a FlyScene to/from the disk.
    /// </summary>
    public class SceneStorage
    {
        private static string SaveFolder
        {
            get
            {
                return Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "Content/Maps" );
            }
        }

        public bool Save( FlyScene scene )
        {
            try
            {
                string directory = SaveFolder;
                Directory.CreateDirectory( directory );

                string fullPath = Path.Combine( directory, "test.flys" );

                using( var stream = new MemoryStream() )
                {
                    var context = new BinarySerializationContext( stream );
                    Save( scene, context );

                    stream.Flush();
                    StorageUtilities.CopyToFile( stream, fullPath );
                }

                Console.WriteLine( "Saved!" );
                return true;
            }
            catch( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return false;
            }
        }

        private void Save( FlyScene scene, BinarySerializationContext context )
        {
            context.WriteDefaultHeader();

            context.Write( scene.HalfSize.X );
            context.Write( scene.HalfSize.Y );
            context.Write( scene.EntityCount );

            foreach( IFlyEntity entity in scene.Entities )
            {
                context.WriteObject( entity );
            }
        }

        public FlyScene Load()
        {
            try
            {
                string fullPath = Path.Combine( SaveFolder, "test.flys" );

                using( var stream = File.OpenRead( fullPath ) )
                {
                    var context = new BinaryDeserializationContext( stream );
                    return Load( context );
                }
            }
            catch( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return null;
            }
        }

        private FlyScene Load( BinaryDeserializationContext context )
        {
            context.ReadDefaultHeader( this.GetType() );

            Point2 halfSize;
            halfSize.X = context.ReadInt32();
            halfSize.Y = context.ReadInt32();

            FlyScene scene = new FlyScene( halfSize );

            int entityCount = context.ReadInt32();

            for( int i = 0; i < entityCount; ++i )
            {
                var entity = context.ReadObject<FlyEntity>();
                scene.AddEntity( entity );
            }

            return scene;
        }
    }
}
