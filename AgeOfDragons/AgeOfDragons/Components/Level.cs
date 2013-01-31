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
    /// TODO: Update summary.
    /// </summary>
    public class Level
    {
        #region Field Region
        #endregion

        #region Property Region

        public int LevelHeight { get; private set; }

        public int LevelWidth { get; private set; }

        public Camera Camera { get; private set; }

        public Map LevelMap { get; private set; }

        public List<PlayerUnit> LevelPlayerUnits { get; set; }

        public List<NPCUnit> LevelNPCUnits { get; set; }

        #endregion

        #region Constructor Region

        public Level(int levelHeight, int levelWidth, Camera camera, Map levelMap, List<PlayerUnit> playerUnits, List<NPCUnit> npcUnits)
        {
            this.LevelHeight = levelHeight;
            this.LevelWidth = levelWidth;
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
