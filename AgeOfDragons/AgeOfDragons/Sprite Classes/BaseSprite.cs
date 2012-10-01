// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseSprite.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that represents a base sprite.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Sprite_Classes
{
    using AgeOfDragons.Tile_Engine;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A class that represents a base sprite.
    /// </summary>
    public class BaseSprite
    {
        #region Field Region

        /// <summary>
        /// The texture of the sprite.
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// The rectangle that determines the size
        /// of the sprite.
        /// </summary>
        private Rectangle sourceRectangle;

        /// <summary>
        /// Determines the position of the sprite.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The velocity of the sprite.
        /// </summary>
        private Vector2 velocity;

        /// <summary>
        /// The speed of the sprite.
        /// </summary>
        private float speed = 2.0f;

        #endregion

        #region Property Region

        /// <summary>
        /// Gets or sets the texture of the sprite.
        /// </summary>
        protected Texture2D Texture
        {
            get { return this.texture; }
            set { this.texture = value; }
        }

        /// <summary>
        /// Gets the width of the sprite.
        /// </summary>
        public int Width
        {
            get { return this.sourceRectangle.Width; }
        }

        /// <summary>
        /// Gets the height of the sprite.
        /// </summary>
        public int Height
        {
            get { return this.sourceRectangle.Height; }
        }

        /// <summary>
        /// Gets a rectangle using the position, width 
        /// and height of the sprite.
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(
                    (int)this.position.X,
                    (int)this.position.Y,
                    this.Width,
                    this.Height);
            }
        }

        /// <summary>
        /// Gets or sets the speed of the sprite.
        /// </summary>
        public float Speed
        {
            get { return this.speed; }
// ReSharper disable ValueParameterNotUsed
            set { this.speed = MathHelper.Clamp(this.speed, 1.0f, 16.0f); }
// ReSharper restore ValueParameterNotUsed
        }

        /// <summary>
        /// Gets or sets the position of the sprite.
        /// </summary>
        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }

        /// <summary>
        /// Gets or sets the velocity of the sprite.
        /// </summary>
        public Vector2 Velocity
        {
            get
            {
                return this.velocity;
            }

            set
            {
                this.velocity = value;
                if (this.velocity != Vector2.Zero)
                {
                    this.velocity.Normalize();
                }
            }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSprite"/> class. 
        /// </summary>
        /// <param name="image"> The image of the sprite. </param>
        /// <param name="sourceRectangle"> The source rectangle of the sprite. </param>
        public BaseSprite(Texture2D image, Rectangle? sourceRectangle)
        {
            this.texture = image;

            if (sourceRectangle.HasValue)
            {
                this.sourceRectangle = sourceRectangle.Value;
            }
            else
            {
                this.sourceRectangle = new Rectangle(
                    0,
                    0,
                    image.Width,
                    image.Height);
            }

            this.position = Vector2.Zero;
            this.velocity = Vector2.Zero;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSprite"/> class.
        /// Sets the position of the sprite by using tiles.
        /// </summary>
        /// <param name="image"> The image of the sprite. </param>
        /// <param name="sourceRectangle"> The source rectangle of the sprite. </param>
        /// <param name="tile"> The tile to set the position of the sprite to. </param>
        public BaseSprite(Texture2D image, Rectangle? sourceRectangle, Point tile) 
            : this(image, sourceRectangle)
        {
            this.position = new Vector2(
                tile.X * Engine.TileWidth,
                tile.Y * Engine.TileHeight);
        }

        #endregion

        #region Method Region

        #endregion

        #region Virtual Method region

        /// <summary>
        /// Updates the sprite.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draws the sprite.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        /// <param name="spriteBatch">
        /// The sprite batch.
        /// </param>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                this.Texture,
                this.Position,
                this.sourceRectangle,
                Color.White);
        }

        #endregion
    }
}
