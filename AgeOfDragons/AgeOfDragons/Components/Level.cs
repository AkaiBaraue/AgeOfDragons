// -----------------------------------------------------------------------
// <copyright file="Level.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace AgeOfDragons.Components
{
    using System.Collections.Generic;

    using AgeOfDragons.Tile_Engine;
    using AgeOfDragons.Units;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// A class that controls an entire level.
    /// </summary>
    public class Level
    {
        #region Field Region
        #endregion

        #region Property Region

        /// <summary>
        /// Gets the camera tied to the level.
        /// </summary>
        public Camera Camera { get; private set; }

        /// <summary>
        /// Gets the map of the level.
        /// </summary>
        public Map LevelMap { get; private set; }

        /// <summary>
        /// Gets or sets the player units in the level.
        /// </summary>
        public List<PlayerUnit> LevelPlayerUnits { get; set; }

        /// <summary>
        /// Gets or sets npc units in the level.
        /// </summary>
        public List<NPCUnit> LevelNPCUnits { get; set; }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="Level"/> class.
        /// </summary>
        /// <param name="camera"> </param>
        /// <param name="levelMap"> </param>
        /// <param name="playerUnits"> </param>
        /// <param name="npcUnits"> </param>
        public Level(Camera camera, Map levelMap, List<PlayerUnit> playerUnits, List<NPCUnit> npcUnits)
        {
            this.Camera = camera;
            this.LevelMap = levelMap;
            this.LevelPlayerUnits = playerUnits;
            this.LevelNPCUnits = npcUnits;
        }

        #endregion

        #region Method Region

        #endregion

        #region Virtual Method region

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            this.Camera.Update(gameTime, this);

            foreach (var playerUnit in this.LevelPlayerUnits)
            {
                playerUnit.Update(gameTime);
            }

            foreach (var npcUnit in this.LevelNPCUnits)
            {
                npcUnit.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            this.LevelMap.DrawMap(gameTime, this);
        }

        #endregion
    }
}
