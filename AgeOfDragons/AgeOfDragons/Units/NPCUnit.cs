// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NPCUnit.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that represents units controlled by the computer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Units
{
    using AgeOfDragons.Pathfinding;
    using AgeOfDragons.Sprite_Classes;
    using AgeOfDragons.Units.Unit_Types;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A class that represents units controlled by the computer.
    /// </summary>
    public class NPCUnit : Unit
    {
        #region Field Region
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region
        
        /// <summary>
        /// Initializes a new instance of the <see cref="NPCUnit"/> class. 
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="x"> The x location of the unit.  </param>
        /// <param name="y"> The y location of the unit.  </param>
        /// <param name="sprite"> The sprite. </param>
        /// <param name="unitClass"> The class of the unit. </param>
        public NPCUnit(string name, int x, int y, AnimatedSprite sprite, UnitClass unitClass)
            : base(name, x, y, sprite, unitClass)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NPCUnit"/> class. 
        /// </summary>
        /// <param name="id"> The id of the unit.  </param>
        /// <param name="name"> The name.  </param>
        /// <param name="position"> The position of the unit.  </param>
        /// <param name="sprite"> The sprite. </param>
        /// <param name="unitClass"> The class of the unit. </param>
        public NPCUnit(int id, string name, Vector position, AnimatedSprite sprite, UnitClass unitClass)
            : base(id, name, position, sprite, unitClass)
        {
        }

        #endregion

        #region Method Region
        #endregion

        #region Virtual Method region
        #endregion
    }
}
