// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlayer.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//    An abstract class for playertypes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Components
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class Player
    {
        /// <summary>
        /// Gets a value indicating whether the player's turn is finished
        /// or not.
        /// </summary>
        public bool IsTurnFinished { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the player is the current
        /// player or not.
        /// </summary>
        public bool IsCurrentPlayer { get; private set; }

        /// <summary>
        /// The method that takes care of stuff that needs to happen at the
        /// start of a player's turn.
        /// </summary>
        public abstract void StartTurn();

        /// <summary>
        /// The method that handles the player's turn.
        /// </summary>
        public abstract void TakeTurn();

        /// <summary>
        /// The method that takes care of stuff that needs to happen when
        /// the player's turn ends.
        /// </summary>
        public abstract void EndTurn();
    }
}
