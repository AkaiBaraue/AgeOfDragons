// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MapData.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//    A map created from MapCells. Holds information like visibility,
//    collisiontype, occupation status, etc.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Tile_Engine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AgeOfDragons.Pathfinding;
    using AgeOfDragons.Units;

    /// <summary>
    /// A map created from MapCells. Holds information like visibility,
    /// collisiontype, occupation status, etc.
    /// </summary>
    public class MapData
    {
        #region Field Region

        /// <summary>
        /// The map.
        /// </summary>
        private MapCell[,] mapData;

        /// <summary>
        /// The height of the map.
        /// </summary>
        private readonly int height;

        /// <summary>
        /// The width of the map.
        /// </summary>
        private readonly int width;

        #endregion

        #region Property Region

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="MapData"/> class.
        /// </summary>
        /// <param name="height"> The height of the MapData. </param>
        /// <param name="width"> The width of the MapData. </param>
        public MapData(int height, int width)
        {
            this.height = height;
            this.width = width;

            this.mapData = new MapCell[height, width];
        }

        #endregion

        #region Method Region

        /// <summary>
        /// Adds collision types to the places on the map where it is needed.
        /// </summary>
        /// <param name="mapLayers"> The map layers the map is created from. </param>
        /// <param name="tilesets"> The tilesets the map is created from. </param>
        public void AddCollision(List<MapLayer> mapLayers, List<Tileset> tilesets)
        {
            // Finds the layer with the name "collision" in the list of layers.
            var collisionLayer = new MapLayer("collision", this.width, this.height);
            foreach (var mapLayer in mapLayers.Where(mapLayer => mapLayer.LayerName.Equals("collision")))
            {
                collisionLayer = mapLayer;
            }

            this.mapData = new MapCell[collisionLayer.Height, collisionLayer.Width];

            // Iterates over all the cells in the collisionlayer and stores their type of
            // collision in the respective cell in the mapData.
            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    var cell = new MapCell
                    {
                        Collision =
                            (CollisionType)
                            (collisionLayer.GetTile(j, i).TileID - tilesets.ToArray()[collisionLayer.GetTile(j, i).TileSet].StartID)
                    };

                    this.mapData[i, j] = cell;
                }
            }
        }

        /// <summary>
        /// Returns whether the given position is occupied by a
        /// unit or not.
        /// </summary>
        /// <param name="x"> The x coordinate of the tile to check. </param>
        /// <param name="y"> The y coordinate of the tile to check. </param>
        /// <returns>
        /// True if the position is occupied.
        /// False if it is not.
        /// </returns>
        public bool IsOccupied(int x, int y)
        {
            return this.mapData[x, y].Occupied;
        }

        /// <summary>
        /// Sets the Occupied property of chosen position to the input boolean
        /// </summary>
        /// <param name="x"> The x coordinate of the tile to check. </param>
        /// <param name="y"> The y coordinate of the tile to check. </param>
        /// <param name="status"> The occupation status to be set. </param>
        public void SetOccupationStatus(int x, int y, bool status)
        {
            this.mapData[x, y].Occupied = status;
        }

        /// <summary>
        /// Returns whether the given position is visible or not
        /// </summary>
        /// <param name="x"> The x coordinate of the tile to check. </param>
        /// <param name="y"> The y coordinate of the tile to check. </param>
        /// <returns>
        /// True if the position is visible.
        /// False if it is not.
        /// </returns>
        public bool IsVisible(int x, int y)
        {
            return this.mapData[x, y].Visible;
        }

        /// <summary>
        /// Sets the Visible property of chosen position to the input boolean
        /// </summary>
        /// <param name="x"> The x coordinate of the tile to check. </param>
        /// <param name="y"> The y coordinate of the tile to check. </param>
        /// <param name="status"> The occupation status to be set. </param>
        public void SetVisibility(int x, int y, bool status)
        {
            this.mapData[x, y].Visible = status;
        }

        /// <summary>
        /// Gets the type of collision for the given position.
        /// </summary>
        /// <param name="x"> The x coordinate of the tile to check. </param>
        /// <param name="y"> The y coordinate of the tile to check. </param>
        /// <returns>
        /// The CollisionType for the tile on position (x, y).
        /// </returns>
        public CollisionType GetCollisionType(int x, int y)
        {
            return this.mapData[x, y].Collision;
        }

        /// <summary>
        /// Checks if a location is within the map, if the location is within the
        /// move range of the unit and if the unit can traverse the position.
        /// </summary>
        /// <param name="x"> The x coordinate to check. </param>
        /// <param name="y"> The y coordinate to check. </param>
        /// <param name="unit"> The unit. </param>
        /// <returns>
        /// True if the above conditions are fulfilled.
        /// </returns>
        public bool WithinMapMoveRangeAndCanTraverse(int x, int y, Unit unit)
        {
            var tempVector = new Vector(x, y);

            if (this.WithinMap(x, y) &&
                unit.PointWithinMoveRange(tempVector))
            {
                var test2 = PathFinding.FindShortestPathWithinReach(unit.Location, tempVector, unit, this);

                if (unit.CanTraverse(this.GetCollisionType(x, y)) &&
                    test2.Count <= unit.MoveRange)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a location is inside the view range of
        /// the unit and within the borders of the DataMap.
        /// </summary>
        /// <param name="x"> The value added to the x coordinate. </param>
        /// <param name="y"> The value added to the y coordinate. </param>
        /// <param name="unit"> The unit. </param>
        /// <returns>
        /// True if the location is within the bounds.
        /// </returns>
        public bool WithinMapAndCanTraverse(int x, int y, Unit unit)
        {
            if (this.WithinMap(x, y))
            {
                if (unit.CanTraverse(this.GetCollisionType(x, y)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a location is inside the view range of
        /// the unit and within the borders of the DataMap.
        /// </summary>
        /// <param name="x"> The value added to the x coordinate. </param>
        /// <param name="y"> The value added to the y coordinate. </param>
        /// <param name="unit"> The unit. </param>
        /// <returns>
        /// True if the location is within the bounds.
        /// </returns>
        public bool WithinViewAndMap(int x, int y, Unit unit)
        {
            if (Math.Abs(x) + Math.Abs(y) <= unit.ViewRange)
            {
                if (!this.WithinMap(x + unit.Location.X, y + unit.Location.Y))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether a certain position is inside the bounds of the map or not.
        /// </summary>
        /// <param name="x"> The x coordinate to check. </param>
        /// <param name="y"> The y coordinate to check. </param>
        /// <returns>
        /// True if the position is within the bounds of the map.
        /// </returns>
        public bool WithinMap(int x, int y)
        {
            if (x < 0 ||
                x > this.height - 1 ||
                y < 0 ||
                y > this.width - 1)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Virtual Method region

        #endregion
    }
}
