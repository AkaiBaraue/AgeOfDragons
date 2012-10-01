// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollisionType.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that represents the different types of collisions
//   that a cell can have.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Tile_Engine
{
    /// <summary>
    /// A class that represents the different types of collisions
    /// that a cell can have.
    /// </summary>
    public enum CollisionType
    {
        /// <summary>
        /// Nothing to collide with
        /// </summary>
        None,

        /// <summary>
        /// The cell can be traversed by flying units and
        /// units that are at home in the water.
        /// (Sea, lake)
        /// </summary>
        Water,

        /// <summary>
        /// The cell can be traversed by flying units and
        /// units that are at home in the mountains.
        /// (Mountain)
        /// </summary>
        Mountain,

        /// <summary>
        /// The cell can only be traversed by flying units.
        /// (Lava)
        /// </summary>
        Flyable,

        /// <summary>
        /// The cell cannot be traversed at all.
        /// (Walls)
        /// </summary>
        Impassable
    }
}
