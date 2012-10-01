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
    using System;
    using System.Collections.Generic;

    using AgeOfDragons.Components;
    using AgeOfDragons.Helper_Classes;
    using AgeOfDragons.Pathfinding;
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
        /// The camera that shows the screen.
        /// </summary>
        private readonly Camera camera;

        /// <summary>
        /// The player that controls the game.
        /// </summary>
        private readonly Player player;

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
        /// The tilemap.
        /// </summary>
        private TileMap map;

        /// <summary>
        /// The spritebatch that takes care of drawing sprites.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// The list of the units controlled by the player.
        /// </summary>
        private List<PlayerUnit> playerUnits;

        /// <summary>
        /// The list of the units controlled by the player.
        /// </summary>
        private List<NPCUnit> npcUnits;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the squares across.
        /// </summary>
        public int SquaresAcross { get; private set; }

        /// <summary>
        /// Gets the squares down.
        /// </summary>
        public int SquaresDown { get; private set; }

        /// <summary>
        /// Gets the map.
        /// </summary>
        public TileMap Map
        {
            get { return this.map; }
        }

        /// <summary>
        /// Gets the camera.
        /// </summary>
        public Camera Camera
        {
            get { return this.camera; }
        }

        /// <summary>
        /// Gets the camera.
        /// </summary>
        public Player Player
        {
            get { return this.player; }
        }

        /// <summary>
        /// Gets the sprite batch.
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return this.spriteBatch; }
        }

        /// <summary>
        /// Gets or sets the player units.
        /// </summary>
        public List<PlayerUnit> PlayerUnits
        {
            get { return this.playerUnits; }
            set { this.playerUnits = value; }
        }

        /// <summary>
        /// Gets or sets the player units.
        /// </summary>
        public List<NPCUnit> NPCUnits
        {
            get { return this.npcUnits; }
            set { this.npcUnits = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Game1"/> class.
        /// </summary>
        public Game1()
        {
            Content.RootDirectory = "Content";

            // Sets the maximum amount of tiles to be shown at one time
            // Across is actually the height of the screen, and down is
            // the width of the screen, but don't think about it. 
            this.SquaresAcross = 11;
            this.SquaresDown = 18;

            // Creates a new GraphicsDeviceManager and sets the height and
            // width to the SquaresDown/Across * the width/height of a tile.
            this.graphics = new GraphicsDeviceManager(this)
                {
                    PreferredBackBufferWidth = this.SquaresDown * Engine.TileWidth,
                    PreferredBackBufferHeight = this.SquaresAcross * Engine.TileHeight
                };

            // Makes a rectangle that is used to tell the Camera how much of the game
            // it is supposed to show at any given time.
            var screenRectangle = new Rectangle(0, 0, this.graphics.PreferredBackBufferHeight, this.graphics.PreferredBackBufferWidth);

            // Initializes player, camera, mapLoader, playerUnits and npcUnits.
            this.player = new Player();
            this.camera = new Camera(screenRectangle);
            this.mapLoader = new MapLoader();
            this.playerUnits = new List<PlayerUnit>();
            this.npcUnits = new List<NPCUnit>();

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
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loads the texture for Fog of War and Valid Move, which are used during the game.
            // Instead of saving it in the tilemap, it is saved with the Engine, as both of them
            // are tilesets consisting of only one tile. No point in saving them as a full tileset in the map.
            Engine.FoWTexture = this.Content.Load<Texture2D>(@"Textures\TileSets\fow_of_war");
            Engine.ValidMoveTexture = this.Content.Load<Texture2D>(@"Textures\TileSets\move_range");

            // Loads a map and saves it for future use.
            this.map = this.mapLoader.LoadTmxFile("Test_1.tmx", this);
            this.map.FowEnabled = true;
            this.map.PersistentFoW = true;

            // Makes an AnimatedSprite from a spritesheet, makes a PlayerUnit from it and
            // adds it to the playerUnits list.
            var sprite = this.MakeSprite("Lin28px", 28, 28);
            var playerUnit = new PlayerUnit("Akai", new Vector(5, 3), sprite, new BladesmasterClass());
            this.playerUnits.Add(playerUnit);

            sprite = this.MakeSprite("FemaleAssassin28px", 28, 28);
            playerUnit = new PlayerUnit("Kitai", new Vector(2, 2), sprite, new AssassinClass());
            this.playerUnits.Add(playerUnit);

            sprite = this.MakeSprite("FlyingUnit32px", 32, 32);
            playerUnit = new PlayerUnit("Flyer", new Vector(7, 12), sprite, new FlyingUnitClass());
            this.playerUnits.Add(playerUnit);

            sprite = this.MakeSprite("FlyingUnit32px", 32, 32);
            var npcUnit = new NPCUnit("Flyer", 10, 10, sprite, new FlyingUnitClass());
            this.npcUnits.Add(npcUnit);

            // Calls the LoadUnits method of the map.
            this.map.LoadUnits(new List<Unit>(this.playerUnits));
            this.map.LoadUnits(new List<Unit>(this.npcUnits));

//            var test = Finder.CostEstimate(new Vector(7, 12), new Vector(5, 13));
//            Console.WriteLine(test);

//            var derp = Finder.FindShortestPathWithinReach(new Vector(7, 12), new Vector(5, 13), playerUnit, this.map.dataMap);
//            Console.WriteLine();
//            Console.Write("[");
//            foreach (var vector in derp)
//            {
//                Console.Write(vector + " FScore: " + vector.FScore + ", ");
//            }
//            Console.WriteLine("]");
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
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || 
                (InputHandler.KeyDown(Keys.LeftShift) && InputHandler.KeyReleased(Keys.F5)))
            {
                this.Exit();
            }

            this.Camera.Update(gameTime, this);
            this.Player.Update(gameTime, this);

            foreach (var playerUnit in this.PlayerUnits)
            {
                playerUnit.Update(gameTime);
            }

            foreach (var npcUnit in this.NPCUnits)
            {
                npcUnit.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this.spriteBatch.Begin();

            this.map.DrawMap(gameTime, this);

            base.Draw(gameTime);

            this.spriteBatch.End();
        }

        #region Non-XNA methods

        /// <summary>
        /// Makes a sprite from the given string.
        /// </summary>
        /// <param name="spriteSheet"> The name of the sprite sheet. </param>
        /// <param name="spriteHeight"> The height of each sprite. </param>
        /// <param name="spriteWidth"> The width of each sprite. </param>
        /// <returns>
        /// An AnimatedSprite based on the spritesheet.
        /// </returns>
        private AnimatedSprite MakeSprite(string spriteSheet, int spriteHeight, int spriteWidth)
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
                this.Content.Load<Texture2D>(@"Textures\Sprites\" + spriteSheet),
                animations);

            return sprite;
        }

        #endregion
    }
}
