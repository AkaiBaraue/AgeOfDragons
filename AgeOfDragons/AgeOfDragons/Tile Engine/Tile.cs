// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tile.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that represents a single tile.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Tile_Engine
{
    /// <summary>
    /// A class that represents a single tile.
    /// </summary>
    public class Tile
    {
        #region Field Region

        /// <summary>
        /// The index of the tile in the TileSet
        /// </summary>
        private int tileID;

        /// <summary>
        /// The index of the TileSet
        /// </summary>
        private int tileSet;

        #endregion

        #region Property Region

        /// <summary>
        /// Gets TileID.
        /// </summary>
        public int TileID
        {
            get { return this.tileID; }
            private set { this.tileID = value; }
        }

        /// <summary>
        /// Gets TileSet.
        /// </summary>
        public int TileSet
        {
            get { return this.tileSet; }
            private set { this.tileSet = value; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="tileID"> The ID of the tile in the tileSet. </param>
        /// <param name="tileSet"> The index of the tileset. </param>
        public Tile(int tileID, int tileSet)
        {
            this.TileID = tileID;
            this.TileSet = tileSet;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tile"/> class.
        /// </summary>
        /// <param name="tileID"> The ID of the tile in the tileSet. </param>
        public Tile(int tileID)
        {
            this.TileID = tileID;
        }

        #endregion
    }
}
