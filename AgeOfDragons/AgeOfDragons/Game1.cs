// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Game1.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   This is the main type for your game
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons
{
    using System.Collections.Generic;

    using AgeOfDragons.Components;
    using AgeOfDragons.Helper_Classes;
    using AgeOfDragons.Pathfinding;
    using AgeOfDragons.Players;
    using AgeOfDragons.Sprite_Classes;
    using AgeOfDragons.Tile_Engine;
    using AgeOfDragons.Units;
    using AgeOfDragons.Units.Unit_Types;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        #region Fields

        /// <summary>
        /// The object used for loading maps from files.
        /// </summary>
        private readonly MapLoader mapLoader;

        /// <summary>
        /// The graphics device manager.
        /// </summary>
        private readonly GraphicsDeviceManager graphics;

        /// <summary>
        /// The tile engine that determines the size of 
        /// tiles.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        private Engine tileEngine = new Engine(32, 32);
        // ReSharper restore UnusedMember.Local

        /// <summary>
        /// An object that makes it possible to synchronize animations
        /// and control them.
        /// </summary>
        private readonly AnimationControl animationControl = new AnimationControl(5, 4);

        /// <summary>
        /// The level currently in progress.
        /// </summary>
        private Level currentLevel;

        /// <summary>
        /// The spritebatch that takes care of drawing sprites.
        /// </summary>
        private static SpriteBatch spriteBatch;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the map.
        /// </summary>
        public Level CurrentLevel
        {
            get { return this.currentLevel; }
            private set { this.currentLevel = value; }
        }

        /// <summary>
        /// Gets the sprite batch.
        /// </summary>
        public static SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        /// <summary>
        /// Gets or sets the font used for text.
        /// </summary>
        public static SpriteFont Font { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Game1"/> class.
        /// </summary>
        public Game1()
        {
            Content.RootDirectory = "Content";

            // Creates a new GraphicsDeviceManager and sets the height and
            // width to the SquaresDown/Across * the width/height of a tile.
            this.graphics = new GraphicsDeviceManager(this)
                {
                    PreferredBackBufferWidth = Engine.SquaresAcross * Engine.TileWidth,
                    PreferredBackBufferHeight = Engine.SquaresDown * Engine.TileHeight
                };

            // Initializes player and mapLoader
            this.mapLoader = new MapLoader();

            // Adds the InputHandler to the components collection, allowing it to
            // update whenever the Game1 class updates.
            this.Components.Add(new InputHandler(this));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // MAkes the mouse show up on the screen.
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Makes a rectangle that is used to tell the Camera how much of the game
            // it is supposed to show at any given time.
            var screenRectangle = new Rectangle(0, 0, this.graphics.PreferredBackBufferHeight, this.graphics.PreferredBackBufferWidth);

            // Loads the texture for Fog of War and Valid Move, which are used during the game.
            // Instead of saving it in the tilemap, it is saved with the Engine, as both of them
            // are tilesets consisting of only one tile. No point in saving them as a full tileset in the map.
            Engine.FoWTexture = this.Content.Load<Texture2D>(@"Textures\TileSets\fow_of_war");
            Engine.ValidMoveTexture = this.Content.Load<Texture2D>(@"Textures\TileSets\move_range");

            Font = this.Content.Load<SpriteFont>(@"Fonts\OwnFont");

            var tempMap = this.mapLoader.LoadTmxFile("Test_1.tmx", this);
            tempMap.FowEnabled = true;
            tempMap.PersistentFoW = true;

            var tempPlayerUnits = new List<Unit>();
            var tempNPCUnits = new List<Unit>();

            // Makes an AnimatedSprite from a spritesheet, makes a PlayerUnit from it and
            // adds it to the playerUnits list.
            var sprite = this.MakeSprite("Ally", "Lin28px", 28, 28);
            var playerUnit = new PlayerUnit(Unit.GenerateUniqueID(tempPlayerUnits), "Akai", new Vector(5, 3), sprite, new BladesmasterClass());
            tempPlayerUnits.Add(playerUnit);

            sprite = this.MakeSprite("Ally", "FemaleAssassin28px", 28, 28);
            playerUnit = new PlayerUnit(Unit.GenerateUniqueID(tempPlayerUnits), "Kitai", new Vector(5, 2), sprite, new AssassinClass());
            tempPlayerUnits.Add(playerUnit);

            sprite = this.MakeSprite("Ally", "FlyingUnit32px", 32, 32);
            playerUnit = new PlayerUnit(Unit.GenerateUniqueID(tempPlayerUnits), "Flyer", new Vector(9, 10), sprite, new FlyingUnitClass());
            tempPlayerUnits.Add(playerUnit);

            sprite = this.MakeSprite("Enemy", "FlyingUnit32px", 32, 32);
            var enemyUnit = new NPCUnit(Unit.GenerateUniqueID(tempNPCUnits), "Flyer", new Vector(10, 10), sprite, new FlyingUnitClass());
            tempNPCUnits.Add(enemyUnit);

            tempMap.LoadUnits(new List<Unit>(tempPlayerUnits));
            tempMap.LoadUnits(new List<Unit>(tempNPCUnits));

            this.CurrentLevel = new Level(
                new Camera(screenRectangle),
                tempMap);
            this.CurrentLevel.AddPlayer(new PlayerHuman(tempPlayerUnits));
            this.CurrentLevel.AddPlayer(new PlayerNPC(tempNPCUnits));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime"> Provides a snapshot of timing values. </param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                (InputHandler.KeyDown(Keys.LeftShift) && InputHandler.KeyReleased(Keys.F5)))
            {
                this.Exit();
            }

            this.animationControl.Update(gameTime);
            this.CurrentLevel.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();

            this.CurrentLevel.Draw(gameTime);

            base.Draw(gameTime);

            SpriteBatch.End();
        }

        #region Non-XNA methods

        /// <summary>
        /// Makes a sprite from the given string.
        /// </summary>
        /// <param name="faction"> "Ally" or "Enemy". </param>
        /// <param name="spriteSheet"> The name of the sprite sheet. </param>
        /// <param name="spriteHeight"> The height of each sprite. </param>
        /// <param name="spriteWidth"> The width of each sprite. </param>
        /// <returns>
        /// An AnimatedSprite based on the spritesheet.
        /// </returns>
        private AnimatedSprite MakeSprite(string faction, string spriteSheet, int spriteHeight, int spriteWidth)
        {
            var animations = new Dictionary<AnimationKey, Animation>();

            var animation = new Animation(4, spriteHeight, spriteWidth, 0, 0);
            animations.Add(AnimationKey.Idle, animation);

            animation = new Animation(4, spriteHeight, spriteWidth, 0, spriteWidth);
            animations.Add(AnimationKey.Selected, animation);

            animation = new Animation(4, spriteHeight, spriteWidth, 0, spriteWidth * 2);
            animations.Add(AnimationKey.Left, animation);

            animation = new Animation(4, spriteHeight, spriteWidth, 0, spriteWidth * 3);
            animations.Add(AnimationKey.Down, animation);

            animation = new Animation(4, spriteHeight, spriteWidth, 0, spriteWidth * 4);
            animations.Add(AnimationKey.Up, animation);

            animation = new Animation(4, spriteHeight, spriteWidth, 0, spriteWidth * 5);
            animations.Add(AnimationKey.Right, animation);

            var sprite = new AnimatedSprite(
                this.Content.Load<Texture2D>(@"Textures\Sprites\" + faction + "\\" + spriteSheet),
                animations);

            return sprite;
        }

        #endregion
    }
}
