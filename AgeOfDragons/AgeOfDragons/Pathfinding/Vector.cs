// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vector.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   Represents a vector.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Pathfinding
{
    /// <summary>
    /// Represents a vector.
    /// </summary>
    public class Vector
    {
        #region Field Region

        /// <summary>
        /// The node the vector came from.
        /// </summary>
        private Vector cameFrom;

        #endregion

        #region Property Region

        /// <summary>
        /// Gets or sets the x coordinate of the vector.
        /// The horizontal value.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate of the vector.
        /// The vertical value.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the g score.
        /// </summary>
        public int GScore { get; set; }

        /// <summary>
        /// Gets or sets the f score.
        /// </summary>
        public int FScore { get; set; }

        /// <summary>
        /// Gets or sets the vectorNode this obkect came from.
        /// </summary>
        public Vector CameFrom
        {
            get { return this.cameFrom; }
            set { this.cameFrom = value; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="x"> The coordinate on the horizontal axis. </param>
        /// <param name="y"> The coordinate on the vertical axis. </param>
        public Vector(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        #endregion

        #region Method Region

        #endregion

        #region Property Region
        #endregion

        #region Virtual Method region

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return this.X ^ this.Y;
        }

        /// <summary>
        /// Determines whether this object is equal to 
        /// the input object.
        /// </summary>
        /// <param name="obj"> The obj. </param>
        /// <returns>
        /// True if they are equal.
        /// </returns>
        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            var tempVec = obj as Vector;
            if (tempVec == null)
            {
                return false;
            }

            return (tempVec.X == this.X) && (tempVec.Y == this.Y);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return "(" + this.X + ", " + this.Y + ")";
        }

        #endregion
    }
}
