// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputHandler.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class for handeling input of different kinds.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Helper_Classes
{
    using System; 

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// A class for handeling input of different kinds.
    /// </summary>
    public class InputHandler : GameComponent
    {
        #region Keyboard Field & Property Region

        /// <summary>
        /// The current state of the keyboard
        /// </summary>
        private static KeyboardState keyboardState;

        /// <summary>
        /// The last state of the keyboard
        /// </summary>
        private static KeyboardState lastKeyboardState;

        /// <summary>
        /// Gets KeyboardState.
        /// </summary>
        public static KeyboardState KeyboardState
        {
            get { return keyboardState; }
        }

        /// <summary>
        /// Gets LastKeyboardState.
        /// </summary>
        public static KeyboardState LastKeyboardState
        {
            get { return lastKeyboardState; }
        }

        #endregion

        #region Mouse Field & Property Region

        /// <summary>
        /// The current state of the keyboard
        /// </summary>
        private static MouseState mouseState;

        /// <summary>
        /// The last state of the keyboard
        /// </summary>
        private static MouseState lastMouseState;

        /// <summary>
        /// Gets KeyboardState.
        /// </summary>
        public static MouseState MouseState
        {
            get { return mouseState; }
        }

        /// <summary>
        /// Gets LastKeyboardState.
        /// </summary>
        public static MouseState LastMouseState
        {
            get { return lastMouseState; }
        }

        #endregion

        #region Game Pad Field & Property Region

        /// <summary>
        /// The state of the different gamepads
        /// </summary>
        private static GamePadState[] gamePadStates;

        /// <summary>
        /// The last state of the different gamepads
        /// </summary>
        private static GamePadState[] lastGamePadStates;

        /// <summary>
        /// Gets GamePadStates.
        /// </summary>
        public static GamePadState[] GamePadStates
        {
            get { return gamePadStates; }
        }

        /// <summary>
        /// Gets LastGamePadStates.
        /// </summary>
        public static GamePadState[] LastGamePadStates
        {
            get { return lastGamePadStates; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="InputHandler"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public InputHandler(Game game)
            : base(game)
        {
            keyboardState = Keyboard.GetState();

            gamePadStates = new GamePadState[Enum.GetValues(typeof(PlayerIndex)).Length];

            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
            {
                gamePadStates[(int)index] = GamePad.GetState(index);
            }
        }

        #endregion

        #region XNA methods

        /// <summary>
        /// Updates lastKeyboardState and keyboardState.
        /// </summary>
        /// <param name="gameTime">
        /// The game time at which the game is updated.
        /// </param>
        public override void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();

            lastMouseState = mouseState;
            mouseState = Mouse.GetState();

            lastGamePadStates = (GamePadState[])gamePadStates.Clone();
            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
            {
                gamePadStates[(int)index] = GamePad.GetState(index);
            }

            base.Update(gameTime);
        }

        #endregion

        #region General Method Region

        /// <summary>
        /// Sets lastKeyboardState to keyboardstate
        /// </summary>
        public static void Flush()
        {
            lastKeyboardState = keyboardState;
        }

        #endregion

        #region Keyboard Region

        /// <summary>
        /// Returns whether keyboardState is not pressed
        /// and lastKeyboardState is pressed at
        /// the same time for the given key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <returns>
        /// True if both conditions hold.
        /// False if not.
        /// </returns>
        public static bool KeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) &&
                   lastKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns whether keyboardState is pressed
        /// and lastKeyboardState is not pressed at
        /// the same time for the given key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <returns>
        /// True if both conditions hold.
        /// False if not.
        /// </returns>
        public static bool KeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) &&
                   lastKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Returns whether keyboardState is being pressed
        /// for the given key.
        /// </summary>
        /// <param name="key"> The key. </param>
        /// <returns>
        /// True if the condition holds.
        /// False if not.
        /// </returns>
        public static bool KeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        #endregion

        #region Mouse Region

        /// <summary>
        /// Returns whether the left mouse button was clicked or not.
        /// </summary>
        /// <returns>
        /// True if both conditions hold.
        /// False if not.
        /// </returns>
        public static bool LeftMouseClicked()
        {
            return mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Returns whether the left mouse button was clicked or not.
        /// </summary>
        /// <returns>
        /// True if both conditions hold.
        /// False if not.
        /// </returns>
        public static bool LeftMouseDown()
        {
            return mouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Returns whether the left mouse button was clicked or not.
        /// </summary>
        /// <returns>
        /// True if both conditions hold.
        /// False if not.
        /// </returns>
        public static bool RightMouseClicked()
        {
            return mouseState.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Returns whether the left mouse button was clicked or not.
        /// </summary>
        /// <returns>
        /// True if both conditions hold.
        /// False if not.
        /// </returns>
        public static bool RightMouseDown()
        {
            return mouseState.RightButton == ButtonState.Pressed;
        }

        #endregion

        #region Game Pad Region

        /// <summary>
        /// Returns whether gamePadStates is not pressed
        /// and lastGamePadStates is pressed at
        /// the same time for the given button.
        /// </summary>
        /// <param name="button">
        /// The button.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// True if both conditions hold.
        /// False if not.
        /// </returns>
        public static bool ButtonReleased(Buttons button, PlayerIndex index)
        {
            return gamePadStates[(int)index].IsButtonUp(button) &&
                   lastGamePadStates[(int)index].IsButtonDown(button);
        }

        /// <summary>
        /// Returns whether gamePadStates is pressed
        /// and lastGamePadStates is not pressed at
        /// the same time for the given button.
        /// </summary>
        /// <param name="button">
        /// The button.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// True if both conditions hold.
        /// False if not.
        /// </returns>
        public static bool ButtonPressed(Buttons button, PlayerIndex index)
        {
            return gamePadStates[(int)index].IsButtonDown(button) &&
                   lastGamePadStates[(int)index].IsButtonUp(button);
        }

        /// <summary>
        /// Returns whether gamePadStates is being 
        /// pressed for the given button.
        /// </summary>
        /// <param name="button">
        /// The button.
        /// </param>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// True if the condition holds.
        /// False if not.
        /// </returns>
        public static bool ButtonDown(Buttons button, PlayerIndex index)
        {
            return gamePadStates[(int)index].IsButtonDown(button);
        }

        #endregion
    }
}