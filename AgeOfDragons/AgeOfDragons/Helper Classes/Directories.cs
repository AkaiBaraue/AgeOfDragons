// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Directories.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class used for accessing certain directories.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Helper_Classes
{
    /// <summary>
    /// A class used for accessing certain directories.
    /// </summary>
    public static class Directories
    {
        #region Field Region
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region
        #endregion

        #region Method Region

        /// <summary>
        /// Gets the directory for the Maps folder in the Content project.
        /// </summary>
        /// <returns> The directory of the Maps folder. </returns>
        public static string GetMapDirection()
        {
            string exeLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string contentLocation = exeLocation.Substring(0, exeLocation.IndexOf("AgeOfDragons\\bin", System.StringComparison.Ordinal));
            return contentLocation + "AgeOfDragonsContent\\Maps\\";
        }

        #endregion

        #region Virtual Method region
        #endregion
    }
}
