// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VectorNode.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that holds a vector and points back to another VectorNode
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Pathfinding
{
    /// <summary>
    /// A class that holds a vector and points back to another VectorNode
    /// </summary>
    public class VectorNode
    {
        #region Field Region

        /// <summary>
        /// The vector.
        /// </summary>
        private Vector vector;

        /// <summary>
        /// The node the vector came from.
        /// </summary>
        private VectorNode cameFrom;

        #endregion

        #region Property Region

        /// <summary>
        /// Gets or sets the vector.
        /// </summary>
        public Vector Vector
        {
            get { return this.vector; }
            set { this.vector = value; }
        }

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
        public VectorNode CameFrom
        {
            get { return this.cameFrom; }
            set { this.cameFrom = value; }
        }

        #endregion

        #region Constructor Region

        #endregion

        #region Method Region

        #endregion

        #region Virtual Method region

        #endregion
    }
}
