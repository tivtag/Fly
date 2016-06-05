// <copyright file="Program.cs" company="federrot Software">
//     Copyright (c) federrot Software. All rights reserved.
// </copyright>
// <summary>
//     Defines the Fly.Program class.
// </summary>
// <author>
//     Paul Ennemoser
// </author>

namespace Fly
{
    /// <summary>
    /// Defines the main entry point of the game.
    /// </summary>
    public sealed class Program
    {
        public static void Main( string[] args )
        {
            using( var game = new FlyGame() )
            {
                game.Run();
            }
        }
    }
}
