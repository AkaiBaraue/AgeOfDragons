// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Engine.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//    An engine that handles the basic tile data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Tile_Engine
{
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// An engine that handles the basic tile data.
    /// </summary>
    public class Engine
    {
        #region Field Region

        /// <summary>
        /// The width of a tile.
        /// </summary>
        private static int tileWidth;

        /// <summary>
        /// The height of a tile.
        /// </summary>
        private static int tileHeight;

        #endregion

        #region Property Region

        /// <summary>
        /// Gets the width of the tile.
        /// </summary>
        public static int TileWidth 
        {
            get { return tileWidth; }
        }

        /// <summary>
        /// Gets the height of the tile.
        /// </summary>
        public static int TileHeight
        {
            get { return tileHeight; }
        }

        /// <summary>
        /// Gets the squares across.
        /// </summary>
        public static int SquaresAcross { get; private set; }

        /// <summary>
        /// Gets the squares down.
        /// </summary>
        public static int SquaresDown { get; private set; }

        /// <summary>
        /// Gets or sets the Fog of War texture.
        /// </summary>
        public static Texture2D FoWTexture { get; set; }

        /// <summary>
        /// Gets or sets the valid move texture.
        /// </summary>
        public static Texture2D ValidMoveTexture { get; set; }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="widthOfTile"> The width of a tile. </param>
        /// <param name="heightOfTile"> The height of a tile. </param>
        /// <param name="squaresAcross"> The amount of squares shown horizontally </param>
        /// <param name="squaresDown"> The amount of squares shown vertically </param>
        public Engine(int widthOfTile, int heightOfTile, int squaresAcross, int squaresDown)
        {
            tileWidth = widthOfTile;
            tileHeight = heightOfTile;
            SquaresAcross = squaresAcross;
            SquaresDown = squaresDown;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Engine"/> class.
        /// </summary>
        /// <param name="widthOfTile"> The width of a tile. </param>
        /// <param name="heightOfTile"> The height of a tile. </param>
        public Engine(int widthOfTile, int heightOfTile)
            : this(widthOfTile, heightOfTile, 18, 11)
        {
        }

        #endregion

        #region Method Region
        #endregion

        #region Virtual Method region
        #endregion
    }
}
