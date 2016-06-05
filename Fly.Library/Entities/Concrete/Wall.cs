// <copyright file="Wall.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Entities.Concrete.Wall class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Entities
{
    using Atom.Math;
    using Atom.Storage;
    using Fly.Components;
    using Fly.Graphics;
    using Fly.Physics.Descriptors;
    using Fly.Saving;

    /// <summary>
    /// Rerpesents a static wall that blocks entities from moving through it.
    /// </summary>
    public sealed class Wall : FlyEntity, IPhysicsEntity, ISaveable
    {
        public SingleFixturePhysicable Physics
        {
            get
            {
                return this.physics;
            }
        }

        IPhysicable IPhysicsEntity.Physics
        {
            get
            {
                return this.physics;
            }
        }

        public Wall()
        {
            this.Components.BeginSetup();
            {
                this.Components.Add( this.physics );
            }
            this.Components.EndSetup();
        }

        public override void Serialize( ISerializationContext context )
        {
            context.WriteDefaultHeader();

            context.Write( this.Position );
            context.Write( this.Rotation );

            context.WriteObject( this.Physics.FixtureDescription );
            context.WriteObject( this.DrawStrategy );
        }

        public override void Deserialize( IDeserializationContext context )
        {
            context.ReadDefaultHeader( typeof( Wall ) );
            this.Position = context.ReadVector2();
            this.Rotation = context.ReadSingle();

            this.Physics.FixtureDescription = context.ReadObject<IPhysicsBodyDescription>();
            this.DrawStrategy = context.ReadObject<IEntityDrawStrategy>();
        }

        private readonly SingleFixturePhysicable physics = new SingleFixturePhysicable();
    }
}
