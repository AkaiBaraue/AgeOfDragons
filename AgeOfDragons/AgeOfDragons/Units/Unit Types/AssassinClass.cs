// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Assassin.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that represents an assassin.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Units.Unit_Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AgeOfDragons.Tile_Engine;

    /// <summary>
    /// A class that represents an assassin.
    /// </summary>
    public class AssassinClass : UnitClass
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
