// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MapCell.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that represents a single cell on the map.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Tile_Engine
{
    /// <summary>
    /// A class that represents a single cell on the map.
    /// </summary>
    public class MapCell
    {
        #region Field Region

        #endregion

        #region Property Region

        /// <summary>
        /// Gets or sets a value indicating whether the cell is visible or not.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cell is occupied or not.
        /// </summary>
        public bool Occupied { get; set; }

        /// <summary>
        /// Gets or sets the collision for the cell.
        /// </summary>
        public CollisionType Collision { get; set; }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="MapCell"/> class.
        /// </summary>
        public MapCell()
        {
            this.Visible = false;
            this.Occupied = false;
            this.Collision = CollisionType.None;
        }

        #endregion

        #region Method Region
        #endregion

        #region Virtual Method region
        #endregion
    }
}
