// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bladesmaster.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   The Bladesmaster class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Units.Unit_Types
{
    using AgeOfDragons.Tile_Engine;

    /// <summary>
    /// The Bladesmaster class.
    /// </summary>
    public class BladesmasterClass : UnitClass
    {
        #region Field Region
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region
        #endregion

        #region Method Region
        #endregion

        #region Virtual Method region
        #endregion

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
        public override bool CanTraverse(CollisionType toTraverse)
        {
            if (toTraverse == CollisionType.None)
            {
                return true;
            }

            return false;
        }
    }
}
