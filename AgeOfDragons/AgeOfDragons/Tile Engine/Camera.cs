// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Camera.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class used for zooming in and out and showing the current position in the world.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Tile_Engine
{
    using AgeOfDragons;
    using AgeOfDragons.Components;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// A class used for zooming in and out and showing the current position in the world.
    /// </summary>
    public class Camera
    {
        #region Field Region

        /// <summary>
        /// The position of the camera.
        /// </summary>
        private Vector2 position;

        /// <summary>
        /// The speed at which the camera moves.
        /// </summary>
        private readonly float speed;

        /// <summary>
        /// The viewport.
        /// </summary>
        private Rectangle viewportRectangle;

        #endregion

        #region Property Region

        /// <summary>
        /// Gets the position of the camera.
        /// </summary>
        public Vector2 Position
        {
            get { return this.position; }
            private set { this.position = value; }
        }

        /// <summary>
        /// Gets the rectangle that represents the viewport.
        /// </summary>
        public Rectangle ViewportRectangle
        {
            get
            {
                return new Rectangle(
                    this.viewportRectangle.X,
                    this.viewportRectangle.Y,
                    this.viewportRectangle.Width,
                    this.viewportRectangle.Height);
            }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        /// <param name="viewportRect"> The viewport rect. </param>
        public Camera(Rectangle viewportRect)
        {
            this.speed = 5f;
            this.viewportRectangle = viewportRect;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        /// <param name="viewportRect"> The viewport rect. </param>
        /// <param name="position"> The position. </param>
        public Camera(Rectangle viewportRect, Vector2 position)
        {
            this.speed = 5f;
            this.viewportRectangle = viewportRect;
            this.Position = position;
        }

        #endregion

        #region Method Region

        /// <summary>
        /// Updates the camera by responding to user input.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="level"> The level in progress. </param>
        public void Update(GameTime gameTime, Level level)
        {
            // Moves the camera to the left, but makes sure it does nto leave the screen.
            if (InputHandler.KeyDown(Keys.Left))
            {
                this.position.X = MathHelper.Clamp(
                    this.Position.X - this.speed, 0, (level.LevelMap.MapWidth - Engine.SquaresAcross) * Engine.TileWidth);
            }

            // Moves the camera to the right, but makes sure it does nto leave the screen.
            if (InputHandler.KeyDown(Keys.Right))
            {
                this.position.X = MathHelper.Clamp(
                    this.Position.X + this.speed, 0, (level.LevelMap.MapWidth - Engine.SquaresAcross) * Engine.TileWidth);
            }

            // Moves the camera up, but makes sure it does nto leave the screen.
            if (InputHandler.KeyDown(Keys.Up))
            {
                this.position.Y = MathHelper.Clamp(
                    this.Position.Y - this.speed, 0, (level.LevelMap.MapHeight - Engine.SquaresDown) * Engine.TileHeight);
            }

            // Moves the camera down, but makes sure it does nto leave the screen.
            if (InputHandler.KeyDown(Keys.Down))
            {
                this.position.Y = MathHelper.Clamp(
                    this.Position.Y + this.speed, 0, (level.LevelMap.MapHeight - Engine.SquaresDown) * Engine.TileHeight);
            }
        }

        #endregion
    }
}
