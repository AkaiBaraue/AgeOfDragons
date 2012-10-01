// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TileSet.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that handles full tilesets
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Tile_Engine
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A class that handles full tilesets
    /// </summary>
    public class Tileset
    {
        #region Fields and Properties

        /// <summary>
        /// Source rectangles for the tileset.
        /// </summary>
        private readonly Rectangle[] sourceRectangles;

        /// <summary>
        /// The lowest ID this tielset takes care of.
        /// </summary>
        private int startID;

        /// <summary>
        /// The highest ID this tileset takes care of.
        /// </summary>
        private int endID;

        #endregion

        #region Property Region

        /// <summary>
        /// Gets Texture.
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        /// Gets TileWidth.
        /// </summary>
        public int TileWidth { get; private set; }

        /// <summary>
        /// Gets TileHeight.
        /// </summary>
        public int TileHeight { get; private set; }

        /// <summary>
        /// Gets TilesWide.
        /// </summary>
        public int TilesWide { get; private set; }

        /// <summary>
        /// Gets TilesHigh.
        /// </summary>
        public int TilesHigh { get; private set; }

        /// <summary>
        /// Gets the start of the tileset.
        /// Used when generating a map from a .tmx file.
        /// </summary>
        public int StartID
        {
            get { return this.startID; }
        }

        /// <summary>
        /// Gets SourceRectangles.
        /// </summary>
        public Rectangle[] SourceRectangles
        {
            get { return (Rectangle[])this.sourceRectangles.Clone(); }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="Tileset"/> class.
        /// </summary>
        /// <param name="image"> The image to make a tileset from. </param>
        /// <param name="tilesWide"> The amount of tiles the picture is wide. </param>
        /// <param name="tilesHigh"> The amount of tiles the picture is high. </param>
        /// <param name="tileWidth"> The width of a single tile. </param>
        /// <param name="tileHeight"> The height of a single tile. </param>
        public Tileset(Texture2D image, int tilesWide, int tilesHigh, int tileWidth, int tileHeight)
        {
            this.Texture = image;
            this.TilesWide = tilesWide;
            this.TilesHigh = tilesHigh;
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;

            int tiles = tilesWide * tilesHigh;
            this.sourceRectangles = new Rectangle[tiles];

            int tile = 0;
            for (int y = 0; y < tilesHigh; y++)
            {
                for (int x = 0; x < tilesWide; x++)
                {
                    this.sourceRectangles[tile] = new Rectangle(
                        x * tileWidth,
                        y * tileHeight,
                        tileWidth,
                        tileHeight);

                    tile++;
                }
            }
        }

        #endregion

        #region Method Region

        /// <summary>
        /// Sets the startID and endID value, which are used to determine whether
        /// a tile has been made with this tileset.
        /// </summary>
        /// <param name="start"> The start ID. </param>
        public void SetFirstgid(int start)
        {
            this.startID = start - 1;
            this.endID = (start - 1) + (this.SourceRectangles.Length - 1);
        }

        /// <summary>
        /// Determines whether a tile belongs to this tileset or not.
        /// </summary>
        /// <param name="tileID"> The ID of the tile that is to be checked. </param>
        /// <returns>
        /// True if the tile is from this tileset.
        /// False if it is not.
        /// </returns>
        public bool FromThisTilset(int tileID)
        {
            if (tileID >= this.startID && tileID <= this.endID)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
