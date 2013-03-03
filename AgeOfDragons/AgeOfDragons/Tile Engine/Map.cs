// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Map.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that takes care of creating a map of tiles, drawing
//   the map, drawing fog of war and drawing units.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Tile_Engine
{
    using System;
    using System.Collections.Generic;

    using AgeOfDragons;
    using AgeOfDragons.Components;
    using AgeOfDragons.Pathfinding;
    using AgeOfDragons.Sprite_Classes;
    using AgeOfDragons.Units;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// A class that takes care of creating a map of tiles, drawing
    /// the map, drawing fog of war and drawing units.
    /// </summary>
    public class Map
    {
        #region Field Region

        /// <summary>
        /// A list of all the tilesets used by the DataMap.
        /// </summary>
        private readonly List<Tileset> tilesets;

        /// <summary>
        /// A list of all the layers used by the DataMap.
        /// </summary>
        private readonly List<MapLayer> mapLayers;

        /// <summary>
        /// A list of the tiles the unit can move to.
        /// </summary>
        private List<Vector> validMovesList;

        /// <summary>
        /// The witdh of the DataMap in tiles.
        /// </summary>
        private static int mapWidth;

        /// <summary>
        /// The height of the DataMap in tiles.
        /// </summary>
        private static int mapHeight;

        /// <summary>
        /// The MapData that holds information about collisiontype,
        /// occupation, etc. of a cell.
        /// </summary>
        private readonly MapData dataMap;

        #endregion

        #region Property Region

        /// <summary>
        /// Gets the witdh of the DataMap in pixels.
        /// </summary>
        public static int WidthInPixels
        {
            get
            {
                return mapWidth * Engine.TileWidth;
            }
        }

        /// <summary>
        /// Gets the height of the DataMap in pixels.
        /// </summary>
        public static int HeightInPixels
        {
            get
            {
                return mapHeight * Engine.TileHeight;
            }
        }

        /// <summary>
        /// Gets the maximum height of the DataMap.
        /// </summary>
        public int MapWidth
        {
            get
            {
                return mapWidth;
            }
        }

        /// <summary>
        /// Gets the maximum height of the DataMap.
        /// </summary>
        public int MapHeight
        {
            get
            {
                return mapHeight;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether fow is enabled or not.
        /// </summary>
        public bool FowEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the FoW is persistent
        /// (comes back when units move away) or not.
        /// </summary>
        public bool PersistentFoW { get; set; }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="tilesets"> The tilesets to create the map from. </param>
        /// <param name="layers"> The layers to create the map from. </param>
        public Map(List<Tileset> tilesets, List<MapLayer> layers)
        {
            this.tilesets = tilesets;
            this.mapLayers = layers;

            mapWidth = this.mapLayers[0].Width;
            mapHeight = this.mapLayers[0].Height;

            // Makes sure that all layers are of the same size.
            for (int i = 1; i < layers.Count; i++)
            {
                if (mapWidth != this.mapLayers[i].Width || mapHeight != this.mapLayers[i].Height)
                {
                    throw new Exception("MapData layer size exception");
                }
            }

            this.dataMap = new MapData(this.MapHeight, this.MapWidth);
            this.dataMap.AddCollision(this.mapLayers, this.tilesets);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class.
        /// </summary>
        /// <param name="tileset"> The tileset to create the map from. </param>
        /// <param name="layer"> The layer to create the map from. </param>
        public Map(Tileset tileset, MapLayer layer)
        {
            this.tilesets = new List<Tileset>();
            this.tilesets.Add(tileset);

            this.mapLayers = new List<MapLayer>();
            this.mapLayers.Add(layer);

            mapWidth = this.mapLayers[0].Width;
            mapHeight = this.mapLayers[0].Height;

            this.dataMap = new MapData(this.MapHeight, this.MapWidth);
            this.dataMap.AddCollision(this.mapLayers, this.tilesets);
        }

        #endregion

        #region Draw Methods Region
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="level"> The level in progress. </param>
        public void DrawMap(GameTime gameTime, Level level)
        {
            this.DrawTiles(level);
            this.DrawUnits(gameTime, level);

            if (this.FowEnabled)
            {
                this.DrawFoW(level);
            }
        }

        /// <summary>
        /// Draws the tiles on the DataMap.
        /// </summary>
        /// <param name="level"> The level in progress. </param>
        private void DrawTiles(Level level)
        {
            // Finds the square that is to the top-left.
            var firstSquare = new Vector2(
                level.Camera.Position.X / Engine.TileWidth,
                level.Camera.Position.Y / Engine.TileHeight);
            var firstX = (int)firstSquare.X;
            var firstY = (int)firstSquare.Y;

            // Figured out what how far the tiles have been offset.
            var squareOffset = new Vector2(
                level.Camera.Position.X % Engine.TileWidth,
                level.Camera.Position.Y % Engine.TileHeight);
            var offsetX = (int)squareOffset.X;
            var offsetY = (int)squareOffset.Y;

            // Iterates over all layers in mapLayers.
            foreach (var mapLayer in this.mapLayers)
            {
                // Ignores the collision layer.
                if (mapLayer.LayerName.Equals("collision"))
                {
                    continue;
                }

                // Iterates over all tiles that are to be drawn on the screen. Ignores all tiles that are
                // outside the bounds of the scrren.
                for (int y = 0; y <= level.Camera.ViewportRectangle.Width / Engine.TileWidth; y++)
                {
                    for (int x = 0; x <= level.Camera.ViewportRectangle.Height / Engine.TileHeight; x++)
                    {
                        // Finds the actual tile
                        int currentX = x + firstX < mapLayer.Width ? x + firstX : mapLayer.Width - 1;
                        int currentY = y + firstY < mapLayer.Height ? y + firstY : mapLayer.Height - 1;

                        Tile tile = mapLayer.GetTile(currentX, currentY);

                        // If the tile is transparent, don't bother drawing it.
                        if (tile.TileID == 0)
                        {
                            continue;
                        }

                        // Draws the tile.
                        Game1.SpriteBatch.Draw(
                            this.tilesets[tile.TileSet].Texture,
                            new Rectangle(
                                (x * Engine.TileWidth) - offsetX,
                                (y * Engine.TileHeight) - offsetY,
                                Engine.TileWidth,
                                Engine.TileHeight),
                            this.tilesets[tile.TileSet].SourceRectangles[tile.TileID],
                            Color.White);
                    }
                }
            }

            // If a unit is selected, the validMovesList is not null.
            if (this.validMovesList != null)
            {
                // Iterates over all vector2d in the valieMovesList
                foreach (var vector2 in this.validMovesList)
                {
                    // Converts the vector to a vector that represents the position on 
                    // the screen.
                    var vect = new Vector2(
                        (vector2.Y * Engine.TileWidth) - level.Camera.Position.X,
                        (vector2.X * Engine.TileHeight) - level.Camera.Position.Y);

                    // Draws the location with a semi-transparent blue square, indicating that
                    // the currently selected unit can walk to those tiles.
                    Game1.SpriteBatch.Draw(
                        Engine.ValidMoveTexture,
                        vect,
                        Color.White);
                }
            }
        }

        /// <summary>
        /// Draws the Fog of War on the map.
        /// </summary>
        /// <param name="level"> The level in progress. </param>
        private void DrawFoW(Level level)
        {
            // Finds the square that is to the top-left.
            var firstSquare = new Vector2(
                level.Camera.Position.X / Engine.TileWidth,
                level.Camera.Position.Y / Engine.TileHeight);
            var firstX = (int)firstSquare.X;
            var firstY = (int)firstSquare.Y;

            // Figured out what how far the tiles have been offset.
            var squareOffset = new Vector2(
                level.Camera.Position.X % Engine.TileWidth,
                level.Camera.Position.Y % Engine.TileHeight);
            var offsetX = (int)squareOffset.X;
            var offsetY = (int)squareOffset.Y;

            // Iterates over all tiles that are to be drawn on the screen. Ignores all tiles that are
            // outside the bounds of the scrren.
            for (int y = 0; y <= level.Camera.ViewportRectangle.Width / Engine.TileWidth; y++)
            {
                for (int x = 0; x <= level.Camera.ViewportRectangle.Height / Engine.TileHeight; x++)
                {
                    // Finds the actual location
                    int currentX = x + firstX < this.MapWidth ? x + firstX : this.MapWidth - 1;
                    int currentY = y + firstY < this.MapHeight ? y + firstY : this.MapHeight - 1;

                    // If the tile is not visible by a unit...
                    if (!this.IsVisible(currentY, currentX))
                    {
                        // ...draw the Fog of War texture on it.
                        Game1.SpriteBatch.Draw(
                            Engine.FoWTexture,
                            new Rectangle(
                                (x * Engine.TileWidth) - offsetX,
                                (y * Engine.TileHeight) - offsetY,
                                Engine.TileWidth,
                                Engine.TileHeight),
                            Color.White);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the unit as a player unit
        /// </summary>
        /// <param name="gameTime"> A snapshot of timing values. </param>
        /// <param name="level"> The current level. </param>
        /// <param name="playerUnit"> The unit </param>
        private void DrawPlayerUnit(GameTime gameTime, Level level, Unit playerUnit)
        {
            var unitOffsetX = (Engine.TileHeight / 2)
                                 - (playerUnit.Sprite.Height / 2);
            var unitOffsetY = (Engine.TileWidth / 2)
                          - (playerUnit.Sprite.Width / 2);

            // Converts the position of the unit to a vector that represents a
            // location on the gamescreen.
            var vect = new Vector2(
                (playerUnit.Sprite.Position.Y * Engine.TileHeight) - level.Camera.Position.X + unitOffsetY,
                (playerUnit.Sprite.Position.X * Engine.TileWidth) - level.Camera.Position.Y + unitOffsetX);

            // Draws the unit.
            playerUnit.Draw(
                gameTime,
                Game1.SpriteBatch,
                vect);
        }

        /// <summary>
        /// Draws the unit as a npc unit
        /// </summary>
        /// <param name="gameTime"> A snapshot of timing values. </param>
        /// <param name="level"> The current level. </param>
        /// <param name="npcUnit"> The unit </param>
        private void DrawNPCUnit(GameTime gameTime, Level level, Unit npcUnit)
        {
            // Draws the NPC is Fog of War is not enabled.
            if (!this.FowEnabled)
            {
                var unitOffsetX = (Engine.TileHeight / 2)
                              - (npcUnit.Sprite.Height / 2);
                var unitOffsetY = (Engine.TileWidth / 2)
                              - (npcUnit.Sprite.Width / 2);

                // Converts the position of the unit to a vector that represents a
                // location on the gamescreen.
                var vect = new Vector2(
                (npcUnit.Sprite.Position.Y * Engine.TileHeight) - level.Camera.Position.X + unitOffsetY,
                (npcUnit.Sprite.Position.X * Engine.TileWidth) - level.Camera.Position.Y + unitOffsetX);

                // Draws the unit.
                npcUnit.Draw(
                    gameTime,
                    Game1.SpriteBatch,
                    vect);

                return;
            }

            // If FoW is enabled and the square the npc unit is on is visible, draw the unit.
            if (this.IsVisible(npcUnit.Location.X, npcUnit.Location.Y))
            {
                var unitOffsetX = (Engine.TileHeight / 2)
                              - (npcUnit.Sprite.Height / 2);
                var unitOffsetY = (Engine.TileWidth / 2)
                              - (npcUnit.Sprite.Width / 2);

                // Converts the position of the unit to a vector that represents a
                // location on the gamescreen.
                var vect = new Vector2(
                (npcUnit.Sprite.Position.Y * Engine.TileHeight) - level.Camera.Position.X + unitOffsetY,
                (npcUnit.Sprite.Position.X * Engine.TileWidth) - level.Camera.Position.Y + unitOffsetX);

                // Draws the unit.
                npcUnit.Draw(
                    gameTime,
                    Game1.SpriteBatch,
                    vect);
            }
        }

        /// <summary>
        /// Draws the units on the DataMap.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="level"> The level in progress. </param>
        private void DrawUnits(GameTime gameTime, Level level)
        {
            // Iterates over every player in the level's playercontroller
            // and draws the units belonging to each player.
            foreach (var player in level.PlayerController.Players)
            {
                foreach (var unit in player.PlayerUnits)
                {
                    if (unit is PlayerUnit)
                    {
                        this.DrawPlayerUnit(gameTime, level, unit);
                    }

                    if (unit is NPCUnit)
                    {
                        this.DrawNPCUnit(gameTime, level, unit);
                    }
                }
            }
        }

        #endregion

        #region Method Region

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
            return this.dataMap.IsOccupied(x, y);
        }

        /// <summary>
        /// Sets the Occupied property of chosen position to the input boolean
        /// </summary>
        /// <param name="x"> The x coordinate of the tile to check. </param>
        /// <param name="y"> The y coordinate of the tile to check. </param>
        /// <param name="status"> The occupation status to be set. </param>
        public void SetOccupationStatus(int x, int y, bool status)
        {
            this.dataMap.SetOccupationStatus(x, y, status);
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
            return this.dataMap.IsVisible(x, y);
        }

        /// <summary>
        /// Sets the Visible property of chosen position to the input boolean
        /// </summary>
        /// <param name="x"> The x coordinate of the tile to check. </param>
        /// <param name="y"> The y coordinate of the tile to check. </param>
        /// <param name="status"> The occupation status to be set. </param>
        public void SetVisibility(int x, int y, bool status)
        {
            this.dataMap.SetVisibility(x, y, status);
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
            return this.dataMap.GetCollisionType(x, y);
        }

        /// <summary>
        /// Loads the units and sets the cells to occupied.
        /// </summary>
        /// <param name="units"> The units. </param>
        public void LoadUnits(List<Unit> units)
        {
            foreach (var unit in units)
            {
                this.SetOccupationStatus(unit.Location.X, unit.Location.Y, true);
                unit.Sprite.CurrentAnimation = AnimationKey.Idle;
                unit.Sprite.IsAnimating = true;

                if (unit is PlayerUnit)
                {
                    Console.WriteLine("Found player unit!");
                    this.RemoveFoW(unit);
                    this.SetVisibility(unit.Location.X, unit.Location.Y, true);
                }
            }
        }

        /// <summary>
        /// Calls the methods that are needed for when a unit is moved away.
        /// </summary>
        /// <param name="unit"> The unit that is moving. </param>
        /// <param name="level"> The level in progress. </param>
        public void MoveUnitAway(Unit unit, Level level)
        {
            this.SetOccupationStatus(unit.Location.X, unit.Location.Y, false);

            if (this.PersistentFoW)
            {
                if (unit is PlayerUnit)
                {
                    this.AddFoW(unit, level);
                }
            }
        }

        /// <summary>
        /// Calls the methods that are needed when a unit moves to a new position.
        /// The unit parameter -must- have been updated with its new position.
        /// </summary>
        /// <param name="unit"> The unit that is moving. </param>
        public void MoveUnitToNewPosition(Unit unit)
        {
            this.SetOccupationStatus(unit.Location.X, unit.Location.Y, true);

            if (unit is PlayerUnit)
            {
                this.RemoveFoW(unit);
            }

            this.validMovesList = null;
        }

        /// <summary>
        /// Adds FoW to the area surrounding the unit.
        /// Mainly used when a unit is moved.
        /// </summary>
        /// <param name="unit"> The unit to add FoW around. </param>
        /// <param name="level"> The level in progress. </param>
        private void AddFoW(Unit unit, Level level)
        {
            if (this.FowEnabled)
            {
                for (int x = 0 - unit.ViewRange; x < unit.ViewRange + 1; x++)
                {
                    for (int y = 0 - unit.ViewRange; y < unit.ViewRange + 1; y++)
                    {
                        if (this.dataMap.WithinViewAndMap(x, y, unit))
                        {
                            this.SetVisibility(unit.Location.X + x, unit.Location.Y + y, false);
                        }
                    }
                }

                foreach (var playerUnit in level.PlayerController.CurrentPlayer.PlayerUnits)
                {
                    playerUnit.HasProcessedFoW = false;
                    this.RemoveFoW(playerUnit);
                }
            }
        }

        /// <summary>
        /// Removes the fog of war surrounding the given unit.
        /// </summary>
        /// <param name="unit"> The unit to remove FoW around. </param>
        private void RemoveFoW(Unit unit)
        {
            if (!unit.HasProcessedFoW
                && this.FowEnabled)
            {
                for (int x = 0 - unit.ViewRange; x < unit.ViewRange + 1; x++)
                {
                    for (int y = 0 - unit.ViewRange; y < unit.ViewRange + 1; y++)
                    {
                        if (this.dataMap.WithinViewAndMap(x, y, unit))
                        {
                            this.SetVisibility(unit.Location.X + x, unit.Location.Y + y, true);
                        }
                    }
                }

                unit.HasProcessedFoW = true;
            }
        }

        /// <summary>
        /// Finds the areas the unit can move to.
        /// </summary>
        /// <param name="unit"> The unit. </param>
        public void MarkValidMoves(Unit unit)
        {
            this.validMovesList = PathFinding.MarkValidMoves(unit, this.dataMap);
        }

        /// <summary>
        /// Clears the list of valid moves.
        /// </summary>
        public void ClearValidMoves()
        {
            this.validMovesList = null;
        }

        /// <summary>
        /// Finds the shortest path for the unit from the unit's position
        /// to the target vector.
        /// </summary>
        /// <param name="unit"> The unit. </param>
        /// <param name="target"> The target position. </param>
        /// <returns>
        /// A list containing the Vectors that make up the shortest path from
        /// the unit's position to the vector.
        /// </returns>
        public List<Vector> FindShortestPathWithinReach(Unit unit, Vector target)
        {
            var start = new Vector(unit.Location.X, unit.Location.Y);
            return PathFinding.FindShortestPathWithinReach(start, target, unit, this.dataMap);
        }

        /// <summary>
        /// Adds a layer to the tilemap
        /// </summary>
        /// <param name="layer"> The layer. </param>
        /// <exception cref="Exception"> An exceptiono </exception>
        public void AddLayer(MapLayer layer)
        {
            if (layer.Width != mapWidth && layer.Height != mapHeight)
            {
                throw new Exception("MapData layer size exception");
            }

            this.mapLayers.Add(layer);
        }

        #endregion
    }
}
