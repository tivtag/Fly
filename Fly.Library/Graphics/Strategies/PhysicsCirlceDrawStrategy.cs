// <copyright file="PhysicsCirlceDrawStrategy.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.Strategies.PhysicsCirlceDrawStrategy class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics.Strategies
{
    using FarseerPhysics.Collision.Shapes;
    using Fly.Components;

    /// <summary>
    /// Implements an <see cref="IEntityDrawStrategy"/> that draws the <see cref="CircleShape"/>d Phyiscs fixture of the entity.
    /// </summary>
    public sealed class PhysicsCirlceDrawStrategy : EntityDrawStrategy
    {
        public int SegmentCount
        {
            get 
            {
                return this.segmentCount; 
            }

            set
            {
                this.segmentCount = value; 
            }
        }        

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
                drawContext.ShapeRenderer.DrawSolidCircle( (CircleShape)this.fixture.Fixture.Shape, this.fixture.PhysicsTransform, this.FinalColor, this.segmentCount );
            }
        }

        private SingleFixturePhysicable fixture;
        private int segmentCount = 32;
    }
}
