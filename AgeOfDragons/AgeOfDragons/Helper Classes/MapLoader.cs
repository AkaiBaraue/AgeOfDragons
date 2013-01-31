// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MapLoader.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class used for loading files and converting them to maps.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Helper_Classes
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using AgeOfDragons.Tile_Engine;

    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A class used for loading files and converting them to maps.
    /// </summary>
    public class MapLoader
    {
        #region Field Region

        /// <summary>
        /// An object that is used for parsing XML to tileMap.
        /// </summary>
        private readonly XMLParser xmlParser;

        /// <summary>
        /// A list of the layers for the map.
        /// </summary>
        private List<MapLayer> layers;

        /// <summary>
        /// A list of the tilesets for the map.
        /// </summary>
        private List<Tileset> tilesets;

        #endregion

        #region Property Region

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="MapLoader"/> class.
        /// </summary>
        public MapLoader()
        {
            this.xmlParser = new XMLParser();
        }

        #endregion

        #region Method Region

        /// <summary>
        /// Loads a Tmx file and makes a TileMap based on it.
        /// </summary>
        /// <param name="mapName"> The name of the map to load. </param>
        /// <param name="gameRef"> A snapshot of the game. </param>
        /// <returns>
        /// The TileMap created from the Tmx file.
        /// </returns>
        public Map LoadTmxFile(string mapName, Game1 gameRef)
        {
            return this.xmlParser.LoadTmxMap(mapName, gameRef);
        }

        /// <summary>
        /// Loads a file and makes a map from it.
        /// </summary>
        /// <param name="fileName"> The name of the file to load. </param>
        /// <param name="gameRef"> A snapshot of the game. </param>
        /// <returns> A TileMap made from the data in the file. </returns>
        public Map LoadTxtFile(string fileName, Game1 gameRef)
        {
            var lines = new List<string>();
            this.layers = new List<MapLayer>();
            this.tilesets = new List<Tileset>();

            // Makes a StreamReader that reads the file.
            using (var file = new StreamReader(fileName))
            {
                string line;

                // Loads all the lines in the file into a list
                while ((line = file.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return this.MakeMap(lines.ToArray(), gameRef);
        }

        /// <summary>
        /// Makes a map based on the parameter input.
        /// </summary>
        /// <param name="lines"> The lines that represent the map. </param>
        /// <param name="gameRef"> A snapshot of the game. </param>
        /// <returns>
        /// A TileMap.
        /// </returns>
        private Map MakeMap(string[] lines, Game1 gameRef)
        {
            int height = 0, width = 0;

            // Finds the height and the width of the map
            foreach (var line in lines)
            {
                string intValue;
                if (line.Contains("height"))
                {
                    intValue = line.Substring(line.IndexOf('=') + 1);
                    height = Convert.ToInt32(intValue);
                }

                if (line.Contains("width"))
                {
                    intValue = line.Substring(line.IndexOf('=') + 1);
                    width = Convert.ToInt32(intValue);
                }
            }

            // Finds all occourences of [layer], and makes a new layer from
            // the data below it.
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("[layer]"))
                {
                    this.MakeLayer(lines, i, height, width, gameRef);
                }
            }

            return new Map(this.tilesets, this.layers);
        }

        /// <summary>
        /// Makes a layer for the map from the given input.
        /// </summary>
        /// <param name="lines"> The lines that holds the data for the map. </param>
        /// <param name="start"> The line to start reading from. </param>
        /// <param name="height"> The height of the map. </param>
        /// <param name="width"> The width of the map. </param>
        /// <param name="gameRef"> A snapshot of the game. </param>
        private void MakeLayer(string[] lines, int start, int height, int width, Game1 gameRef)
        {
            // Finds the name of the tileset used to make this specific layer and loads it.
            var tilesetName = lines[start + 2].Substring(lines[start + 2].IndexOf('=') + 1);
            var tilesetTexture = gameRef.Content.Load<Texture2D>(@"Textures\TileSets\" + tilesetName);
            var backgroundTileset = new Tileset(
                tilesetTexture, tilesetTexture.Width / 32, tilesetTexture.Height / 32, 32, 32);
            this.tilesets.Add(backgroundTileset);

            var index = lines[start + 1].IndexOf('=') + 1;

            var layerName = lines[start + 1].Substring(index, lines[start + 1].Length - index);

            var background = new MapLayer(layerName, width, height);

            // Goes through all the lines that hold tileIDs
            for (int i = 0; i < height; i++)
            {
                // Loads the current line and splits it into a string array.
                string[] stringTileIDs = lines[i + start + 4].Split(',');
                var tileIDs = new int[width];

                // Converts each ID from a string to an int.
                for (int j = 0; j < width; j++)
                {
                    tileIDs[j] = Convert.ToInt32(stringTileIDs[j]);
                }

                // Loops through the list of TileIDs and places new tiles in the layer, corrosponding
                // to the ID and position.
                for (int j = 0; j < width; j++)
                {
                    background.SetTile(i, j, new Tile(tileIDs[j] - 1 < 0 ? 0 : tileIDs[j] - 1, this.tilesets.Count - 1));
                }
            }

            this.layers.Add(background);
        }

        #endregion

        #region Virtual Method region
        #endregion
    }
}
