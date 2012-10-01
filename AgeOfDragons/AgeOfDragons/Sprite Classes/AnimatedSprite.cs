// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimatedSprite.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class for animated sprites.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Sprite_Classes
{
    using System.Collections.Generic;

    using AgeOfDragons.Pathfinding;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A class for animated sprites.
    /// </summary>
    public class AnimatedSprite
    {
        #region Field Region
        
        /// <summary>
        /// Maps each animation to an AnimationKey to make it easier to find
        /// a certain animation at a later point.
        /// </summary>
        private readonly Dictionary<AnimationKey, Animation> animations;

        /// <summary>
        /// The texture of the sprite.
        /// </summary>
        private readonly Texture2D texture;
        
        /// <summary>
        /// The current animation.
        /// </summary>
        private AnimationKey currentAnimation;
        
        /// <summary>
        /// Decides if the animation is running or not.
        /// </summary>
        private bool isAnimating;

        /// <summary>
        /// The position of the animated sprite.
        /// </summary>
        private Vector position;

        /// <summary>
        /// The speed of tha animated sprite.
        /// </summary>
        private float speed = 1.0f;

        #endregion

        #region Property Region

        /// <summary>
        /// Gets or sets the current animation key.
        /// </summary>
        public AnimationKey CurrentAnimation
        {
            get { return this.currentAnimation; }
            set { this.currentAnimation = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the animation is
        /// running or not.
        /// </summary>
        public bool IsAnimating
        {
            get { return this.isAnimating; }
            set { this.isAnimating = value; }
        }

        /// <summary>
        /// Gets the width of the animated sprite.
        /// </summary>
        public int Width
        {
            get { return this.animations[this.currentAnimation].FrameWidth; }
        }

        /// <summary>
        /// Gets the height of the animated sprite..
        /// </summary>
        public int Height
        {
            get { return this.animations[this.currentAnimation].FrameHeight; }
        }

        /// <summary>
        /// Gets or sets the speed of the animation.
        /// </summary>
        public float Speed
        {
            get { return this.speed; }
// ReSharper disable ValueParameterNotUsed
            set { this.speed = MathHelper.Clamp(this.speed, 1.0f, 16.0f); }
// ReSharper restore ValueParameterNotUsed
        }

        /// <summary>
        /// Gets or sets the position of the animation.
        /// </summary>
        public Vector Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimatedSprite"/> class.
        /// </summary>
        /// <param name="sprite"> The texture of the sprite. </param>
        /// <param name="animation"> The mapping of animationkeys to animations. </param>
        public AnimatedSprite(Texture2D sprite, Dictionary<AnimationKey, Animation> animation)
        {
            this.texture = sprite;
            this.animations = new Dictionary<AnimationKey, Animation>();

            foreach (AnimationKey key in animation.Keys)
            {
                this.animations.Add(key, (Animation)animation[key].Clone());
            }
        }

        #endregion

        #region Method Region

        /// <summary>
        /// Updates the animated sprite.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            if (this.isAnimating)
            {
                this.animations[this.currentAnimation].Update(gameTime);
            }
        }

        /// <summary>
        /// Updates the animated sprite.
        /// </summary>
        public void ResetAnimation()
        {
            this.animations[this.currentAnimation].Reset();
        }

        /// <summary>
        /// Draws the animated sprite.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="spriteBatch"> The sprite batch. </param>
        /// <param name="vect"> The vector position. </param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 vect)
        {
            spriteBatch.Draw(
                this.texture,
                vect,
                this.animations[this.currentAnimation].CurrentFrameRect,
                Color.White);
        }

        #endregion
    }
}