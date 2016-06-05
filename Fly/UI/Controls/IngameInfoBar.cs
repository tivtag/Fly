// <copyright file="IngameInfoBar.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.UI.IngameInfoBar class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly.UI
{
    using System;
    using Atom.Math;
    using Atom.Xna.Fonts;
    using Xna = Microsoft.Xna.Framework;

    /// <summary>
    /// Draws text-based ingame information such as the state of the empire, the player ship and keyboard controls.
    /// </summary>
    public sealed class IngameInfoBar : PlayerUIElement
    {
        protected override void OnDraw( IFlyDrawContext drawContext )
        {
            DrawSpeed( drawContext );
            DrawEmpireStats( drawContext );
            DrawControlInfo( drawContext );
        }

        private void DrawSpeed( IFlyDrawContext drawContext )
        {
            Vector2 velocity = this.Player.Physics.LinearVelocity;
            double speed = Math.Round( velocity.Length );

            this.font.Draw( speed.ToString() + " km/s", new Vector2( 55, 10 ), Xna.Color.LightGreen, drawContext );
        }

        private void DrawEmpireStats( IFlyDrawContext drawContext )
        {
            var empire = this.Player.OwnedBy.Empire;
            string str = string.Format( "{0} Minerals", empire.Minerals.ToString() );

            this.font.Draw( str, new Vector2( drawContext.ViewWidth - 15, 10 ), TextAlign.Right, Xna.Color.LightGreen, drawContext );
        }

        private void DrawControlInfo( IFlyDrawContext drawContext )
        {
            this.font.Draw( "[Shift -> Boost]       [Q -> Quantum Stop]       [Esc -> Exit]",   new Vector2( 15, drawContext.ViewHeight - 70 ), TextAlign.Left, Xna.Color.LightGreen, drawContext );
            this.font.Draw( "[SPACE -> Bullet]      [B -> Tractor Beam]       [F10 -> Editor]", new Vector2( 15, drawContext.ViewHeight - 50 ), TextAlign.Left, Xna.Color.LightGreen, drawContext );
            this.font.Draw( "[ALT   -> Rocket]      [X -> Blackhole]",                          new Vector2( 15, drawContext.ViewHeight - 30 ), TextAlign.Left, Xna.Color.LightGreen, drawContext );
        }

        private readonly IFont font = UIFonts.Quartz10;
    }
}
