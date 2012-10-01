// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   Defines the Program type and starts the game.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons
{
#if WINDOWS || XBOX

    /// <summary>
    /// The main program that starts the game.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            using (var game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}