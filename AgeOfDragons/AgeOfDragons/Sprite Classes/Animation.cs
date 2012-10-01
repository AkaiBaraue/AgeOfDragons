// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Animation.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   a class that takes care of animation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Sprite_Classes
{
    using System;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// a class that takes care of animation.
    /// </summary>
    public class Animation : ICloneable
    {
        #region Field Region
        /// <summary>
        /// The frames used for the animation.
        /// </summary>
        private readonly Rectangle[] frames;
        
        /// <summary>
        /// The frames that the animation shows per second.
        /// </summary>
        private int framesPerSecond;

        /// <summary>
        /// The length of a frame.
        /// </summary>
        private TimeSpan frameLength;
        
        /// <summary>
        /// The time that has passed since the last frame was shown.
        /// </summary>
        private TimeSpan frameTimer;
        
        /// <summary>
        /// The current frame shown.
        /// </summary>
        private int currentFrame;

        /// <summary>
        /// The 'physical' width of a frame on the sprite sheet.
        /// </summary>
        private int frameWidth;
        
        /// <summary>
        /// The 'physical' height of a frame on the sprite sheet.
        /// </summary>
        private int frameHeight;

        #endregion

        #region Property Region

        /// <summary>
        /// Gets or sets the frames per second of the animation.
        /// </summary>
        public int FramesPerSecond
        {
            get
            {
                return this.framesPerSecond;
            }

            set
            {
                if (value < 1)
                {
                    this.framesPerSecond = 1;
                }
                else if (value > 60)
                {
                    this.framesPerSecond = 60;
                }
                else
                {
                    this.framesPerSecond = value;
                }
                
                this.frameLength = TimeSpan.FromSeconds(1 / (double)this.framesPerSecond);
            }
        }

        /// <summary>
        /// Gets the rectangle for the current frame of the animation.
        /// </summary>
        public Rectangle CurrentFrameRect
        {
            get { return this.frames[this.currentFrame]; }
        }

        /// <summary>
        /// Gets or sets the current frame of the animation.
        /// </summary>
        public int CurrentFrame
        {
            get
            {
                return this.currentFrame;
            }

            set
            {
                this.currentFrame = (int)MathHelper.Clamp(value, 0, this.frames.Length - 1);
            }
        }

        /// <summary>
        /// Gets the width of the frames.
        /// </summary>
        public int FrameWidth
        {
            get { return this.frameWidth; }
        }

        /// <summary>
        /// Gets the height of the frames.
        /// </summary>
        public int FrameHeight
        {
            get { return this.frameHeight; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        /// <param name="frameCount"> The number of frames of the animation. </param>
        /// <param name="frameWidth"> The width of each frame. </param>
        /// <param name="frameHeight"> The height of each frame. </param>
        /// <param name="xOffset"> The x offset. </param>
        /// <param name="yOffset"> The y offset. </param>
        public Animation(int frameCount, int frameWidth, int frameHeight, int xOffset, int yOffset)
        {
            this.frames = new Rectangle[frameCount];
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;

            for (int i = 0; i < frameCount; i++)
            {
                this.frames[i] = new Rectangle(
                xOffset + (frameWidth * i),
                yOffset,
                frameWidth,
                frameHeight);
            }

            this.FramesPerSecond = 5;
            this.Reset();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Animation"/> class.
        /// </summary>
        /// <param name="animation">
        /// The animation.
        /// </param>
        private Animation(Animation animation)
        {
            this.frames = animation.frames;
            this.FramesPerSecond = 5;
        }

        #endregion

        #region Method Region

        /// <summary>
        /// Updates the animation
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            this.frameTimer += gameTime.ElapsedGameTime;
            if (this.frameTimer >= this.frameLength)
            {
                this.frameTimer = TimeSpan.Zero;
                this.currentFrame = (this.currentFrame + 1) % this.frames.Length;
            }
        }

        /// <summary>
        /// Resets the animation.
        /// </summary>
        public void Reset()
        {
            this.currentFrame = 0;
            this.frameTimer = TimeSpan.Zero;
        }

        #endregion

        #region Interface Method Region

        /// <summary>
        /// Clones the animation.
        /// </summary>
        /// <returns>
        /// The cloned animation as an object.
        /// </returns>
        public object Clone()
        {
            var animationClone = new Animation(this);
            animationClone.frameWidth = this.frameWidth;
            animationClone.frameHeight = this.frameHeight;
            animationClone.Reset();

            return animationClone;
        }

        #endregion
    }
}