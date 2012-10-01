// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitClass.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   Determines the type of the unit, and through that what kind of 
//   CollisionType it can traverse.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Units.Unit_Types
{
    using AgeOfDragons.Tile_Engine;

    /// <summary>
    /// Determines the type of the unit, and through that what kind of 
    /// CollisionType it can traverse.
    /// </summary>
    public abstract class UnitClass
    {
        /// <summary>
        /// Indicates whether the class can traverse this CollisionType or not.
        /// </summary>
        /// <param name="toTraverse">
        /// The CollisionType to traverse.
        /// </param>
        /// <returns>
        /// True if the unit can traverse it.
        /// False if the unit cannot.
        /// </returns>
        public abstract bool CanTraverse(CollisionType toTraverse);
    }
}
