// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Player.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that represents the player in the game
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Components
{
    using System;

    using AgeOfDragons.Pathfinding;
    using AgeOfDragons.Tile_Engine;
    using AgeOfDragons.Units;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// A class that represents the player in the game
    /// </summary>
    public class Player
    {
        #region Field Region

        /// <summary>
        /// Indicates whether a unit is currently selected or not.
        /// </summary>
        private bool unitSelected;

        /// <summary>
        /// Prevents multiple Update actions from happening at the same time.
        /// </summary>
        private bool justClicked;

        /// <summary>
        /// The location of the selected unit.
        /// </summary>
        private PlayerUnit selectedUnit;

        #endregion

        #region Property Region

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        public Player()
        {
            this.unitSelected = false;
        }

        #endregion

        #region Method Region

        /// <summary>
        /// Updates the player.
        /// </summary>
        /// <param name="gameTime"> The game time. </param>
        /// <param name="gameRef"> A snapshot of the game. </param>
        public void Update(GameTime gameTime, Game1 gameRef)
        {
            this.justClicked = false;

            if (InputHandler.LeftMouseClicked() &&
                !this.unitSelected &&
                !this.justClicked)
            {
                // Note! MouseState.X is the horizontal position and MouseState.Y is the vertical position.
                // I want Y to be the horizontal coordinate (the width) and X to be the vertical coordinate
                // (the height), so tileY is using the X values and tileX is using the Y values.
                var tileY = (int)(InputHandler.MouseState.X + gameRef.Camera.Position.X) / Engine.TileHeight;
                var tileX = (int)(InputHandler.MouseState.Y + gameRef.Camera.Position.Y) / Engine.TileWidth;

                foreach (var playerUnit in gameRef.PlayerUnits)
                {
                    // Checks if the unit matches the tile that was clicked.
                    if (playerUnit.Location.X == tileX &&
                        playerUnit.Location.Y == tileY)
                    {
                        // Selects the unit.
                        playerUnit.Select();
                        this.unitSelected = true;
                        this.selectedUnit = playerUnit;
                        gameRef.Map.MarkValidMoves(this.selectedUnit);
                    }
                }

                this.justClicked = true;
            }

            if (InputHandler.LeftMouseClicked() && 
                this.unitSelected &&
                !this.justClicked)
            {
                // Note! MouseState.X is the horizontal position and MouseState.Y is the vertical position.
                // I want Y to be the horizontal coordinate (the width) and X to be the vertical coordinate
                // (the height), so tileY is using the X values and tileX is using the Y values.
                var tileY = (int)(InputHandler.MouseState.X + gameRef.Camera.Position.X) / Engine.TileHeight;
                var tileX = (int)(InputHandler.MouseState.Y + gameRef.Camera.Position.Y) / Engine.TileWidth;

                // Checks if the chosen tile is occupied or not.
                if (!gameRef.Map.IsOccupied(tileX, tileY) &&
                    this.selectedUnit.CanTraverse(gameRef.Map.GetCollisionType(tileX, tileY)) &&
                    (Math.Abs(tileX - this.selectedUnit.Location.X) + Math.Abs(tileY - this.selectedUnit.Location.Y) <= this.selectedUnit.MoveRange))
                {
                    var pathToTarget = gameRef.Map.ShortestPath(new Vector(tileX, tileY), this.selectedUnit).Count;
                    if (pathToTarget != 0 &&
                        pathToTarget <= this.selectedUnit.MoveRange)
                    {
                        // Removes the unit from the list, as we need to change the
                        // position of the unit.
                        gameRef.PlayerUnits.Remove(this.selectedUnit);

                        // Notifies the map that the unit is disappearing from its current
                        // location.
                        gameRef.Map.MoveUnitAway(this.selectedUnit, gameRef);

                        // Changes the unit's location.
                        this.selectedUnit.Location = new Vector(tileX, tileY);
                        this.selectedUnit.MoveUnit();

                        // Notifies the map that the unit has arrived at its target
                        // location and adds the unit to the unit list again.
                        gameRef.Map.MoveUnitToNewPosition(this.selectedUnit);
                        gameRef.PlayerUnits.Add(this.selectedUnit);

                        this.unitSelected = false;
                        this.selectedUnit = null;
                    }
                }

                this.justClicked = true;
            }
        }

        #endregion
    }
}