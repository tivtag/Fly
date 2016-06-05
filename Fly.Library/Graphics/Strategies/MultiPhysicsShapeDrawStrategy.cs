// <copyright file="MultiPhysicsShapeDrawStrategy.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Graphics.Strategies.MultiPhysicsShapeDrawStrategy class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.Graphics.Strategies
{
    using FarseerPhysics.Dynamics;
    using Fly.Components;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Implements an <see cref="IEntityDrawStrategy"/> that draws the Phyiscs fixtures of the entity.
    /// </summary>
    public class MultiPhysicsShapeDrawStrategy : EntityDrawStrategy
    {
        protected override void OnEntityChanged()
        {
            if( this.Entity != null )
            {
                this.fixture = this.Entity.Components.Get<MultiFixturePhysicable>();
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
                Xna.Color color = this.FinalColor;

                foreach( Fixture fixture in this.fixture.Fixtures )
                {
                    FarseerPhysics.Common.Transform transform;
                    fixture.Body.GetTransform( out transform );

                    drawContext.ShapeRenderer.DrawShape( fixture, transform, color );                    
                }
            }
        }

        private MultiFixturePhysicable fixture;
    }
}
