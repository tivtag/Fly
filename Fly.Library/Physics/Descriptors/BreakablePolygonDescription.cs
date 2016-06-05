
// <copyright file="BreakableCircleDescription.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Physics.Descriptors.BreakablePolygonDescription class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Physics.Descriptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Atom.Math;
    using Atom.Xna;
    using FarseerPhysics.Common;
    using FarseerPhysics.Common.Decomposition;
    using FarseerPhysics.Dynamics;

    /// <summary>
    /// Implements an IMultiPhysicsBodyDescription that builds new Polygon-shaped physics objects
    /// with a specific configuration that can break into multiple parts.
    /// </summary>
    public class BreakablePolygonDescription : BaseMultiPhysicsBodyDescription
    {
        public Vertices Vertices 
        {
            get;
            set;
        }

        public override Tuple<IEnumerable<Fixture>, Body, object> Build( World world )
        {
            BreakableBody breakableBody = CreateBreakableBody( world, this.Vertices, this.Density );
            this.InitBody( breakableBody.MainBody );
            
            return Tuple.Create<IEnumerable<Fixture>, Body, object>( breakableBody.Parts, breakableBody.MainBody, breakableBody );
        }

        public static BreakableBody CreateBreakableBody( World world, Vertices vertices, float density)
        {
            List<Vertices> triangles;

            if( vertices.Count <= 10 )
            {
                triangles = DelauneyTriangulate( vertices );
            }
            else
            {
                if( rand.RandomBoolean )
                {
                    triangles = DelauneyTriangulate( vertices );
                }
                else
                {
                    triangles = EarclipDecomposer.ConvexPartition( vertices );
                }
            }

            BreakableBody breakableBody = new BreakableBody( triangles, world, density );
            world.AddBreakableBody( breakableBody );

            return breakableBody;
        }

        private static List<Vertices> DelauneyTriangulate( Vertices vertices )
        {
            var tris = Atom.Math.DelaunyTriangulation.Triangulate( vertices.Select( v => v.ToAtom() ).ToList() );

            List<Vertices> triangles = GetTris( tris, vertices );
            return triangles;
        }

        private static List<FarseerPhysics.Common.Vertices> GetTris( IList<Atom.Math.IndexedTriangle> tris, FarseerPhysics.Common.Vertices vertices )
        {
            List<FarseerPhysics.Common.Vertices> res = new List<Vertices>();

            foreach( var tri in tris )
            {
                var v = new Vertices( new Microsoft.Xna.Framework.Vector2[] { vertices[tri.IndexA], vertices[tri.IndexB], vertices[tri.IndexC] } );
 
                bool b = v.IsCounterClockWise();

                if( !v.IsCounterClockWise() )
                {
                    v.Reverse();
                }

                res.Add(  v );
            }

            return res;
        }

        private static readonly RandMT rand = new RandMT();
    }
}
