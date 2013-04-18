// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerHuman.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that represents the player in the game
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Players
{
    using System;
    using System.Collections.Generic;

    using AgeOfDragons.Components;
    using AgeOfDragons.Pathfinding;
    using AgeOfDragons.Tile_Engine;
    using AgeOfDragons.Units;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// A class that represents the human player in the game.
    /// </summary>
    public class PlayerHuman : Player
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
        private Unit selectedUnit;

        #endregion

        #region Property Region

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerHuman"/> class.
        /// </summary>
        public PlayerHuman()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerHuman"/> class.
        /// </summary>
        /// <param name="units"> The units belonging to the player. </param>
        public PlayerHuman(List<Unit> units)
        {
            this.PlayerUnits = units;
            this.unitSelected = false;
        }

        #endregion

        #region Virtual Method Region

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

            if (!this.IsCurrentPlayer)
            {
                return;
            }

            this.justClicked = false;

            // Just testing stuff
            if (InputHandler.KeyPressed(Keys.K))
            {
                var random = new Random();
                var toChange = random.Next(this.PlayerUnits.Count);
                var unit = this.PlayerUnits[toChange];
                unit.CurrentHealth = unit.CurrentHealth / 2;

                this.PlayerUnits.Remove(unit);
                level.LevelMap.MoveUnitAway(unit, level);
            }

            this.SelectUnit(level);
            this.DeselectUnit(level);
            this.MoveUnit(level);
            this.EndPlayersTurn(level);
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

        #endregion

        #region Method Region

        /// <summary>
        /// Checks if a unit belonging to the player is at the clicked location
        /// and selects it if there is.
        /// </summary>
        /// <param name="level"> The level. </param>
        private void SelectUnit(Level level)
        {
            if (InputHandler.LeftMouseClicked()
                && !this.unitSelected
                && !this.justClicked)
            {
                // Note! MouseState.X is the horizontal position and MouseState.Y is the vertical position.
                // I want Y to be the horizontal coordinate (the width) and X to be the vertical coordinate
                // (the height), so tileY is using the X values and tileX is using the Y values.
                var tileY = (int)(InputHandler.MouseState.X + level.Camera.Position.X) / Engine.TileHeight;
                var tileX = (int)(InputHandler.MouseState.Y + level.Camera.Position.Y) / Engine.TileWidth;

                foreach (var playerUnit in this.PlayerUnits)
                {
                    // Checks if the unit matches the tile that was clicked.
                    if (playerUnit.Location.X == tileX && playerUnit.Location.Y == tileY)
                    {
                        // Selects the unit and thereby changes the animation of the unit.
                        playerUnit.Select();
                        this.unitSelected = true;
                        this.selectedUnit = playerUnit;

                        // Finds the area which the unit can walk to.
                        level.LevelMap.MarkValidMoves(this.selectedUnit);
                    }
                }

                // Ensures that other parts in the update method won't be triggered.
                this.justClicked = true;
            }
        }

        /// <summary>
        /// Deselects the currently selected unit.
        /// </summary>
        /// <param name="level"> The level. </param>
        private void DeselectUnit(Level level)
        {
            if (InputHandler.RightMouseClicked()
                && this.unitSelected
                && !this.justClicked)
            {
                level.LevelMap.ClearValidMoves();
                this.selectedUnit.Deselect();

                this.unitSelected = false;
                this.selectedUnit = null;

                // Ensures that other parts in the update method won't be triggered.
                this.justClicked = true;
            }
        }

        /// <summary>
        /// Moves the selected unit to the clicked location, assuming the location
        /// is within range of the unit.
        /// </summary>
        /// <param name="level"> The level. </param>
        private void MoveUnit(Level level)
        {
            if (InputHandler.LeftMouseClicked()
                && this.unitSelected
                && !this.justClicked)
            {
                // Note! MouseState.X is the horizontal position and MouseState.Y is the vertical position.
                // I want Y to be the horizontal coordinate (the width) and X to be the vertical coordinate
                // (the height), so tileY is using the X values and tileX is using the Y values.
                var tileY = (int)(InputHandler.MouseState.X + level.Camera.Position.X) / Engine.TileHeight;
                var tileX = (int)(InputHandler.MouseState.Y + level.Camera.Position.Y) / Engine.TileWidth;

                var tempVector = new Vector(tileX, tileY);

                // Checks if the chosen tile is occupied or not.
                if (!level.LevelMap.IsOccupied(tileX, tileY)
                    && this.selectedUnit.CanTraverse(level.LevelMap.GetCollisionType(tileX, tileY))
                    && this.selectedUnit.PointWithinMoveRange(tempVector))
                {
                    var pathToTarget = level.LevelMap.FindShortestPathWithinReach(this.selectedUnit, tempVector).Count;
                    if (pathToTarget != 0 && pathToTarget <= this.selectedUnit.MoveRange)
                    {
                        // Removes the unit from the list, as we need to change the
                        // position of the unit.
                        this.PlayerUnits.Remove(this.selectedUnit);

                        // Notifies the map that the unit is disappearing from its current
                        // location.
                        level.LevelMap.MoveUnitAway(this.selectedUnit, level);

                        // Changes the unit's location.
                        this.selectedUnit.Location = (Vector)tempVector.Clone();
                        this.selectedUnit.MoveUnit();

                        // Notifies the map that the unit has arrived at its target
                        // location and adds the unit to the unit list again.
                        level.LevelMap.MoveUnitToNewPosition(this.selectedUnit);
                        this.PlayerUnits.Add(this.selectedUnit);

                        this.unitSelected = false;
                        this.selectedUnit = null;
                    }
                }

                // Ensures that other parts in the update method won't be triggered.
                this.justClicked = true;
            }
        }

        /// <summary>
        /// Ends the player's turn
        /// </summary>
        /// <param name="level"> The level. </param>
        private void EndPlayersTurn(Level level)
        {
            if (InputHandler.KeyPressed(Keys.G))
            {
                this.IsTurnFinished = true;

                this.selectedUnit.MoveUnit();
                level.LevelMap.MoveUnitToNewPosition(this.selectedUnit);
                this.unitSelected = false;
                this.selectedUnit = null;

                // Ensures that other parts in the update method won't be triggered.
                this.justClicked = true;
            }
        }

        #endregion
    }
}