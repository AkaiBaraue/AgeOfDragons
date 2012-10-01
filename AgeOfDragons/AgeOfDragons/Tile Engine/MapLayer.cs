// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MapLayer.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that represents a layer of the map.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Tile_Engine
{
    /// <summary>
    /// A class that represents a layer of the map.
    /// </summary>
    public class MapLayer
    {
        #region Field Region

        /// <summary>
        /// Represents the map as Tiles
        /// </summary>
        private readonly Tile[,] map;

        #endregion

        #region Property Region

        /// <summary>
        /// Gets or sets the layer name.
        /// </summary>
        public string LayerName { get; set; }

        /// <summary>
        /// Gets Width.
        /// </summary>
        public int Width
        {
            get { return this.map.GetLength(1); }
        }

        /// <summary>
        /// Gets Height.
        /// </summary>
        public int Height
        {
            get { return this.map.GetLength(0); }
        }

        /// <summary>
        /// Gets the map.
        /// </summary>
        public Tile[,] Map
        {
            get { return this.map; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="MapLayer"/> class.
        /// </summary>
        /// <param name="layerName"> The name of the layer. </param>
        /// <param name="map"> The map to make a layer from. </param>
        public MapLayer(string layerName, Tile[,] map)
        {
            this.LayerName = layerName;
            this.map = (Tile[,])map.Clone();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapLayer"/> class.
        /// Creates an empty map.
        /// </summary>
        /// <param name="layerName"> The name of the layer. </param>
        /// <param name="width"> The width of the maplayer. </param>
        /// <param name="height"> The height of the maplayer. </param>
        public MapLayer(string layerName, int width, int height)
        {
            this.LayerName = layerName;
            this.map = new Tile[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    this.map[y, x] = new Tile(0, 0);
                }
            }
        }

        #endregion

        #region Method Region

        /// <summary>
        /// Gets the tile at the specified location.
        /// </summary>
        /// <param name="x"> The x coordinate of the tile's potision in the map. </param>
        /// <param name="y"> The y coordinate of the tile's position in the map. </param>
        /// <returns>
        /// The tile on the given position
        /// </returns>
        public Tile GetTile(int x, int y)
        {
            return this.map[y, x];
        }

        /// <summary>
        /// Sets the specified position to the tile.
        /// </summary>
        /// <param name="x"> The x coordinate of the tile's potision in the map. </param>
        /// <param name="y"> The y coordinate of the tile's potision in the map. </param>
        /// <param name="tile"> The tile to insert into the map. </param>
        public void SetTile(int x, int y, Tile tile)
        {
            this.map[y, x] = tile;
        }

        /// <summary>
        /// Sets the specified position a tile with the given
        /// index and tileset.
        /// </summary>
        /// <param name="x"> The x coordinate of the tile's potision in the map. </param>
        /// <param name="y"> The y coordinate of the tile's potision in the map. </param>
        /// <param name="tileIndex"> The tile index. </param>
        /// <param name="tileset"> The tileset. </param>
        public void SetTile(int x, int y, int tileIndex, int tileset)
        {
            this.map[y, x] = new Tile(tileIndex, tileset);
        }

        /// <summary>
        /// Makes a string that represents the layer.
        /// </summary>
        /// <returns>
        /// A string
        /// </returns>
        public override string ToString()
        {
            string mapIDs = string.Empty;

            for (int i = 0; i < this.map.GetLength(1); i++)
            {
                for (int j = 0; j < this.map.GetLength(0); j++)
                {
                    mapIDs += " " + this.map[i, j].TileID;
                }

                mapIDs += "\r\n";
            }

            return mapIDs;
        }

        #endregion
    }
}
