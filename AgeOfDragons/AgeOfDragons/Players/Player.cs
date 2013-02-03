// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="Baraue">
//   o
// </copyright>
// <summary>
//   An abstract class for playertypes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Players
{
    using AgeOfDragons.Components;
    using AgeOfDragons.Units;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The base of all players in the game.
    /// </summary>
    /// <typeparam name="T">
    /// The type of units the player holds.
    /// </typeparam>
    public abstract class Player
    {
        /// <summary>
        /// Gets or sets a value indicating whether the player's turn is finished
        /// or not.
        /// </summary>
        public bool IsTurnFinished { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player is the current
        /// player or not.
        /// </summary>
        public bool IsCurrentPlayer { get; protected set; }

        /// <summary>
        /// Updates the player.
        /// </summary>
        /// <param name="gameTime"> Provides a snapshot of timing values. </param>
        /// <param name="level"> The level in progress. </param>
        public abstract void Update(GameTime gameTime, Level level);

        /// <summary>
        /// The method that takes care of stuff that needs to happen at the
        /// start of a player's turn.
        /// </summary>
        public abstract void StartTurn();

        /// <summary>
        /// The method that takes care of stuff that needs to happen when
        /// the player's turn ends.
        /// </summary>
        public abstract void EndTurn();
    }
}
