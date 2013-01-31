// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimationControl.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that makes it easier to control animations
//   over multiple different units.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Sprite_Classes
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// A class that makes it easier to control animations
    /// over multiple different units.
    /// </summary>
    public class AnimationControl
    {
        #region Field Region
        #endregion

        #region Property Region

        /// <summary>
        /// Gets or sets the number of the current frame to display for an AnimatedSprite.
        /// </summary>
        public static int CurrentFrame { get; set; }

        /// <summary>
        /// Gets or sets the frames per second for animated sprites.
        /// </summary>
        public static int FramesPerSecond { get; set; }

        /// <summary>
        /// Gets or sets the amount of frames that make up a full animation.
        /// </summary>
        public static int FramesInAnimation { get; set; }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimationControl"/> class.
        /// </summary>
        /// <param name="framesPerSecond"> The amount of frames per second. </param>
        /// <param name="framesInAnimation"> The amount of frames used to make a full animation. </param>
        public AnimationControl(int framesPerSecond, int framesInAnimation)
        {
            CurrentFrame = 0;
            FramesPerSecond = framesPerSecond;
            FramesInAnimation = framesInAnimation;
        }

        #endregion

        #region Method Region

        /// <summary>
        /// Updates the AnimationControl.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            CurrentFrame = ((int)gameTime.TotalGameTime.TotalMilliseconds / (1000 / FramesPerSecond)) % FramesInAnimation;
        }

        #endregion

        #region Virtual Method region
        #endregion
    }
}
