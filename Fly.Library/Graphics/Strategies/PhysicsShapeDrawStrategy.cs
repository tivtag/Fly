// <copyright file="PhysicsShapeDrawStrategy.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.Strategies.PhysicsShapeDrawStrategy class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics.Strategies
{
    using Fly.Components;

    /// <summary>
    /// Implements an <see cref="IEntityDrawStrategy"/> that draws the Phyiscs fixture of the entity.
    /// </summary>
    public class PhysicsShapeDrawStrategy : EntityDrawStrategy
    {
        protected override void OnEntityChanged()
        {
            if( this.Entity != null )
            {
                this.fixture = this.Entity.Components.Get<SingleFixturePhysicable>();
            }
            else
            {
                this.fixture = null;
            }
        }

        public override void Draw( IFlyDrawContext drawContext )
        {
            if( this.fixture != null )
            {
                drawContext.ShapeRenderer.DrawShape( this.fixture.Fixture, this.fixture.PhysicsTransform, this.FinalColor );
            }
        }

        private SingleFixturePhysicable fixture;
    }
}
