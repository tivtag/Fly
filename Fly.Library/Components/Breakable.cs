// <copyright file="Breakable.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Components.Breakable class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Components
{
    using System.Linq;
    using Atom;
    using FarseerPhysics.Dynamics;

    /// <summary>
    /// Adds a mechanism for being able to break into multiple parts to this entity.
    /// An entity might break on destruction (<see cref="ILifeStatusComponent"/>).
    /// </summary>
    public sealed class Breakable : FlyComponent
    {
        public event SimpleEventHandler<Breakable> Broken;

        public bool IsBroken
        {
            get
            {
                return this.breakableBody.Broken;
            }
        }

        public override void InitializeBindings()
        {
            this.physics = this.Owner.Components.Get<MultiFixturePhysicable>();
            this.physics.Generated += this.OnPhysicsGenerated;
        }

        private void OnPhysicsGenerated( IPhysicable sender )
        {
            this.breakableBody = (BreakableBody)this.physics.FixtureData;
            this.breakableBody.Decomposed += this.OnBreakableBodyDecomposed;
        }

        private void OnBreakableBodyDecomposed( object sender, System.EventArgs e )
        {
            var fixtures = this.breakableBody.DecomposedBodies.Select( b => b.FixtureList[0] ).ToArray();
            this.physics.Fixtures = fixtures;
        }

        public void Break()
        {
            if( !this.IsBroken )
            {
                this.breakableBody.Break();
                this.Broken.Raise( this );
            }
        }

        private MultiFixturePhysicable physics;
        private BreakableBody breakableBody;
    }
}
