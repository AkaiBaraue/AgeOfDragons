// -----------------------------------------------------------------------
// <copyright file="PlayerController.cs" company="Baraue">
//   o
// </copyright>
// <summary>
//   A class that represents the player in the game
// </summary>
// -----------------------------------------------------------------------

namespace AgeOfDragons.Players
{
    using System.Collections.Generic;
    using System.Globalization;

    using AgeOfDragons.Components;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// Controls the players.
    /// </summary>
    public class PlayerController
    {
        #region Field Region

        /// <summary>
        /// The players.
        /// </summary>
        private LinkedList<Player> players;

        /// <summary>
        /// The current player.
        /// </summary>
        private LinkedListNode<Player> currentPlayer;

        #endregion

        #region Property Region

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController"/> class.
        /// </summary>
        public PlayerController()
        {
            this.players = new LinkedList<Player>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerController"/> class.
        /// </summary>
        /// <param name="player"> The player to add to the list. </param>
        public PlayerController(Player player)
        {
            this.players = new LinkedList<Player>();
            this.AddPlayer(player);
        }

        #endregion

        #region Method Region

        /// <summary>
        /// Adds a player to the controller.
        /// </summary>
        /// <param name="player"> The player to add to the list. </param>
        public void AddPlayer(Player player)
        {
            this.players.AddLast(new LinkedListNode<Player>(player));

            if (this.currentPlayer == null)
            {
                this.currentPlayer = this.players.First;
                this.currentPlayer.Value.StartTurn();
            }
        }

        #endregion

        #region Virtual Method region

        /// <summary>
        /// Updates the Controller.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="level">The level currently in progress.</param>
        public void Update(GameTime gameTime, Level level)
        {
            foreach (var player in this.players)
            {
                player.Update(gameTime, level);
            }

            if (this.currentPlayer.Value.IsTurnFinished)
            {
                this.currentPlayer.Value.EndTurn();
                this.currentPlayer = this.currentPlayer.Next ?? this.players.First;
                this.currentPlayer.Value.StartTurn();
            }
        }

        /// <summary>
        /// Draws text
        /// </summary>
        /// <param name="gameTime"> Provides a snapshot of timing values. </param>
        public void Draw(GameTime gameTime)
        {
            string text;
            string time = gameTime.TotalGameTime.Seconds.ToString(CultureInfo.InvariantCulture);

            if (this.currentPlayer.Value is PlayerHuman)
            {
                text = "Player";
            }
            else
            {
                text = "NPC";
            }

            Game1.SpriteBatch.DrawString(
                Game1.Font, text, new Vector2(10, 10), Color.Black);

            Game1.SpriteBatch.DrawString(
                Game1.Font, time, new Vector2(10, 40), Color.Black);
        }

        #endregion
    }
}
