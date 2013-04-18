// -----------------------------------------------------------------------
// <copyright file="PlayerNPC.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that represents a npc player.
// </summary>
// -----------------------------------------------------------------------

namespace AgeOfDragons.Players
{
    using System.Collections.Generic;

    using AgeOfDragons.Components;
    using AgeOfDragons.Units;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// A class that represents a npc player in the game.
    /// </summary>
    public class PlayerNPC : Player
    {
        #region Field Region
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerNPC"/> class.
        /// </summary>
        public PlayerNPC()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerNPC"/> class.
        /// </summary>
        /// <param name="units"> The units belonging to the player. </param>
        public PlayerNPC(List<Unit> units)
        {
            this.PlayerUnits = units;
        }

        #endregion

        #region Method Region
        #endregion

        #region Virtual Method region
        #endregion

        /// <summary>
        /// Updates the player.
        /// </summary>
        /// <param name="gameTime"> Provides a snapshot of timing values. </param>
        /// <param name="level"> The level in progress. </param>
        public override void Update(GameTime gameTime, Level level)
        {
            foreach (var playerUnit in this.PlayerUnits)
            {
                playerUnit.Update(gameTime);
            }

            // Stupid AI
            // TODO: Improve
            if (gameTime.TotalGameTime.Seconds % 10 == 0)
            {
                this.IsTurnFinished = true;   
            }
        }

        /// <summary>
        /// The method that takes care of stuff that needs to happen at the
        /// start of a player's turn.
        /// </summary>
        public override void StartTurn()
        {
            this.IsTurnFinished = false;
            this.IsCurrentPlayer = true;
        }

        /// <summary>
        /// The method that takes care of stuff that needs to happen when
        /// the player's turn ends.
        /// </summary>
        public override void EndTurn()
        {
            this.IsCurrentPlayer = false;
        }
    }
}
