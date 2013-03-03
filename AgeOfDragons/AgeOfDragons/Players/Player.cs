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
    using System.Collections.Generic;

    using AgeOfDragons.Components;
    using AgeOfDragons.Units;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The base of all players in the game.
    /// </summary>
    public abstract class Player
    {
        /// <summary>
        /// Gets or sets the list of units belonging to the player.
        /// </summary>
        public List<Unit> PlayerUnits { get; protected set; } 

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

        /// <summary>
        /// Adds multiple units to the list of units the player has
        /// </summary>
        /// <param name="units"> The units to add. </param>
        public void AddUnits(List<Unit> units)
        {
            foreach (var unit in units)
            {
                this.AddUnit(unit);
            }
        }

        /// <summary>
        /// Adds a single unit to the list of units the player has
        /// </summary>
        /// <param name="unit"> The unit to add. </param>
        public void AddUnit(Unit unit)
        {
            this.PlayerUnits.Add(unit);
        }

        /// <summary>
        /// Removes multiple units from the list of units the player has
        /// </summary>
        /// <param name="units"> The units to remove. </param>
        public void RemoveUnits(List<Unit> units)
        {
            foreach (var unit in units)
            {
                this.RemoveUnit(unit);
            }
        }

        /// <summary>
        /// Removes a single unit to the list of units the player has
        /// </summary>
        /// <param name="unit"> The unit to remove. </param>
        public void RemoveUnit(Unit unit)
        {
            this.PlayerUnits.Remove(unit);
        }
    }
}
