// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HumanPlayer.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that represents the player in the game
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Components
{
    using AgeOfDragons.Pathfinding;
    using AgeOfDragons.Tile_Engine;
    using AgeOfDragons.Units;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// A class that represents the player in the game
    /// </summary>
    public class HumanPlayer : Player
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
        /// Initializes a new instance of the <see cref="HumanPlayer"/> class.
        /// </summary>
        public HumanPlayer()
        {
            this.unitSelected = false;
        }

        #endregion

        #region Method Region

        /// <summary>
        /// Updates the player.
        /// </summary>
        /// <param name="gameTime"> The game time. </param>
        /// <param name="level"> The level in progress. </param>
        public void Update(GameTime gameTime, Level level)
        {
            this.justClicked = false;

            if (InputHandler.LeftMouseClicked() &&
                !this.unitSelected &&
                !this.justClicked)
            {
                // Note! MouseState.X is the horizontal position and MouseState.Y is the vertical position.
                // I want Y to be the horizontal coordinate (the width) and X to be the vertical coordinate
                // (the height), so tileY is using the X values and tileX is using the Y values.
                var tileY = (int)(InputHandler.MouseState.X + level.Camera.Position.X) / Engine.TileHeight;
                var tileX = (int)(InputHandler.MouseState.Y + level.Camera.Position.Y) / Engine.TileWidth;

                foreach (var playerUnit in level.LevelPlayerUnits)
                {
                    // Checks if the unit matches the tile that was clicked.
                    if (playerUnit.Location.X == tileX &&
                        playerUnit.Location.Y == tileY)
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

            if (InputHandler.LeftMouseClicked() && 
                this.unitSelected &&
                !this.justClicked)
            {
                // Note! MouseState.X is the horizontal position and MouseState.Y is the vertical position.
                // I want Y to be the horizontal coordinate (the width) and X to be the vertical coordinate
                // (the height), so tileY is using the X values and tileX is using the Y values.
                var tileY = (int)(InputHandler.MouseState.X + level.Camera.Position.X) / Engine.TileHeight;
                var tileX = (int)(InputHandler.MouseState.Y + level.Camera.Position.Y) / Engine.TileWidth;

                var tempVector = new Vector(tileX, tileY);

                // Checks if the chosen tile is occupied or not.
                if (!level.LevelMap.IsOccupied(tileX, tileY) &&
                    this.selectedUnit.CanTraverse(level.LevelMap.GetCollisionType(tileX, tileY)) &&
                    this.selectedUnit.PointWithinMoveRange(tempVector))
                {
                    var pathToTarget = level.LevelMap.FindShortestPathWithinReach(this.selectedUnit, tempVector).Count;
                    if (pathToTarget != 0 &&
                        pathToTarget <= this.selectedUnit.MoveRange)
                    {
                        // Removes the unit from the list, as we need to change the
                        // position of the unit.
                        level.LevelPlayerUnits.Remove(this.selectedUnit);

                        // Notifies the map that the unit is disappearing from its current
                        // location.
                        level.LevelMap.MoveUnitAway(this.selectedUnit, level);

                        // Changes the unit's location.
                        this.selectedUnit.Location = (Vector)tempVector.Clone();
                        this.selectedUnit.MoveUnit();

                        // Notifies the map that the unit has arrived at its target
                        // location and adds the unit to the unit list again.
                        level.LevelMap.MoveUnitToNewPosition(this.selectedUnit);
                        level.LevelPlayerUnits.Add(this.selectedUnit);

                        this.unitSelected = false;
                        this.selectedUnit = null;
                    }
                }

                // Ensures that other parts in the update method won't be triggered.
                this.justClicked = true;
            }
        }

        #endregion

        #region Virtual Method Region

        public override void StartTurn()
        {
            throw new System.NotImplementedException();
        }

        public override void TakeTurn()
        {
            throw new System.NotImplementedException();
        }

        public override void EndTurn()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}