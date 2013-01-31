// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMLParser.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   A class that can parse XML data to TileMap files.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Helper_Classes
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Xml;

    using AgeOfDragons.Tile_Engine;

    using Ionic.Zlib;

    using Microsoft.Xna.Framework.Graphics;

    using CompressionMode = System.IO.Compression.CompressionMode;
    using GZipStream = System.IO.Compression.GZipStream;

    /// <summary>
    /// A class that can parse XML data to TileMap files.
    /// </summary>
    public class XMLParser
    {
        #region Field Region

        /// <summary>
        /// The height and width of the map.
        /// </summary>
        private int mapHeight, mapWidth;

        /// <summary>
        /// The list of tilesets for the map.
        /// </summary>
        private List<Tileset> tilesetList = new List<Tileset>();

        /// <summary>
        /// The list of layers that make up the map.
        /// </summary>
        private List<MapLayer> layerList = new List<MapLayer>(); 

        #endregion

        #region Property Region
        #endregion

        #region Constructor Region
        #endregion

        #region Method Region

        /// <summary>
        /// Loads a .tmx map and converts it to a TileMap.
        /// </summary>
        /// <param name="mapName"> The name of the map to load. </param>
        /// <param name="game"> The game. </param>
        /// <returns>
        /// A TileMap based on the .tmx map.
        /// </returns>
        public Map LoadTmxMap(string mapName, Game1 game)
        {
            // Finds the path to the map.
            var filename = Path.Combine(Directories.GetMapDirection(), mapName);

            this.tilesetList = new List<Tileset>();
            this.layerList = new List<MapLayer>();

            // Creates a new XmlDocument and loads the map in as one such.
            var document = new XmlDocument();
            document.Load(filename);

            XmlNode mapNode = document["map"];

            // Asserts that certain things exists in the XMLdocument.
            Debug.Assert(mapNode != null, "mapNode != null");
            Debug.Assert(mapNode.Attributes != null, "mapNode.Attributes != null");
            Debug.Assert(mapNode.FirstChild.Attributes != null, "mapNode.FirstChild.Attributes != null");

            // Finds the height and the width of the map.
            this.mapWidth = int.Parse(mapNode.Attributes["width"].Value, CultureInfo.InvariantCulture);
            this.mapHeight = int.Parse(mapNode.Attributes["height"].Value, CultureInfo.InvariantCulture);

            var xmlNodeList = document.SelectNodes("map/tileset");
            if (xmlNodeList != null)
            {
                // Loops through each node that describes a tileset.
                foreach (XmlNode tilesetNode in xmlNodeList)
                {
                    // Asserts that the tilesetNode contains attributes and that the
                    // first child contains attributes.
                    Debug.Assert(tilesetNode.Attributes != null, "tilesetNode.Attributes != null");
                    Debug.Assert(tilesetNode.FirstChild.Attributes != null, "tilesetNode.FirstChild.Attributes != null");

                    if (tilesetNode.FirstChild.Attributes["source"] != null)
                    {
                        // Loads the name, texture, height and width of the current tilesetNode.
                        var tilesetName = tilesetNode.Attributes["name"].Value;
                        var tilesetTexture = game.Content.Load<Texture2D>(@"Textures\TileSets\" + tilesetName);
                        var tilesWide = int.Parse(tilesetNode.FirstChild.Attributes["width"].Value)
                                        / int.Parse(tilesetNode.Attributes["tilewidth"].Value);
                        var tilesHigh = int.Parse(tilesetNode.FirstChild.Attributes["height"].Value)
                                        / int.Parse(tilesetNode.Attributes["tileheight"].Value);

                        // Makes a new tileset from the above variables.
                        var testset = new Tileset(
                            tilesetTexture, 
                            tilesWide, 
                            tilesHigh,
                            int.Parse(tilesetNode.Attributes["tilewidth"].Value),
                            int.Parse(tilesetNode.Attributes["tileheight"].Value));

                        // Sets the Firstgid value of the tilset to the value stored in the tilesetNode.
                        testset.SetFirstgid(int.Parse(tilesetNode.Attributes["firstgid"].Value));

                        // Adds the tileset to the list of tilesets that make up the map.
                        this.tilesetList.Add(testset);
                    }
                }
            }

            var selectNodes = document.SelectNodes("map/layer|map/objectgroup");
            if (selectNodes != null)
            {
                // Loops through each node that describes a layer in the XMLDocument
                foreach (XmlNode layerNode in selectNodes)
                {
                    if (layerNode.Name == "layer")
                    {
                        // Makes a layer from the node.
                        this.MakeLayer(layerNode);
                    }
                }
            }

            // Creates and returns a TileMap based on the list of tilesets and maplayers.
            return new Map(this.tilesetList, this.layerList);
        }

        /// <summary>
        /// Makes a layer based on the given XmlNode.
        /// </summary>
        /// <param name="node"> The node to make a layer from. </param>
        /// <exception cref="Exception"> An exception that tells that the encoding is invalid. </exception>
        private void MakeLayer(XmlNode node)
        {
            // Asserts that the node contains attributes.
            Debug.Assert(node.Attributes != null, "node.Attributes != null");

            XmlNode dataNode = node["data"];

            // Creates a layer from the name attribute in the dataNode.
            var layer = new MapLayer(node.Attributes["name"].Value, this.mapWidth, this.mapHeight);
            var layerData = new int[this.mapWidth * this.mapHeight];

            // Asserts that certain nodes needed to make tha layer exists.
            Debug.Assert(dataNode != null, "dataNode != null");
            Debug.Assert(dataNode.Attributes != null, "dataNode.Attributes != null");

            // Figure out what encoding is being used, if any, and process
            // the data appropriately
            if (dataNode.Attributes["encoding"] != null)
            {
                string encoding = dataNode.Attributes["encoding"].Value;

                if (encoding == "base64")
                {
                    this.ReadAsBase64(layerData, node, dataNode);
                }
                else if (encoding == "csv")
                {
                    this.ReadAsCsv(layerData, node);
                }
                else
                {
                    throw new Exception("Unknown encoding: " + encoding);
                }
            }
            else
            {
                // Load all tiles with value 0.
                for (int i = 0; i < layerData.Length; i++)
                {
                    layerData[i] = 0;
                }
            }

            this.LoadIntoLayer(layerData, layer);

            this.layerList.Add(layer);
        }

        /// <summary>
        /// Reads an XmlNode as Csv.
        /// </summary>
        /// <param name="layerData"> The layer data. </param>
        /// <param name="node"> The node to process. </param>
        private void ReadAsCsv(int[] layerData, XmlNode node)
        {
            // Split the text up into lines
            string[] lines = node.InnerText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // Iterate each line
            for (int i = 0; i < lines.Length; i++)
            {
                // Split the line into individual pieces
                string[] indices = lines[i].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // Iterate the indices and store in our data
                for (int j = 0; j < indices.Length; j++)
                {
                    layerData[(i * this.mapWidth) + j] = int.Parse(indices[j], CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>
        /// Reads an XmlNode as Base64 and decodes it.
        /// </summary>
        /// <param name="layerData"> The layer data. </param>
        /// <param name="node"> The node to decompress. </param>
        /// <param name="dataNode"> The node that holds data about compression type. </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the compression is not gzip or zlib.
        /// </exception>
        private void ReadAsBase64(int[] layerData, XmlNode node, XmlNode dataNode)
        {
            // Get a stream to the decoded Base64 text
            Stream data = new MemoryStream(Convert.FromBase64String(node.InnerText), false);

            // Figure out what, if any, compression we're using. the compression determines
            // if we need to wrap our data stream in a decompression stream
            Debug.Assert(dataNode.Attributes != null, "dataNode.Attributes != null");
            if (dataNode.Attributes["compression"] != null)
            {
                string compression = dataNode.Attributes["compression"].Value;

                if (compression == "gzip")
                {
                    // Changes the stream to a GZipStream.
                    data = new GZipStream(data, CompressionMode.Decompress, false);
                }
                else if (compression == "zlib")
                {
                    // Changes the stream to a ZlibStream.
                    data = new ZlibStream(data, Ionic.Zlib.CompressionMode.Decompress);
                }
                else
                {
                    throw new InvalidOperationException("Unknown compression: " + compression);
                }
            }

            // Read in all the integers
            using (data)
            {
                using (var reader = new BinaryReader(data))
                {
                    for (int i = 0; i < layerData.Length; i++)
                    {
                        layerData[i] = reader.ReadInt32();
                    }
                }
            }
        }

        /// <summary>
        /// Converts all the tileIDs into Tiles and plots
        /// them into the layer.
        /// </summary>
        /// <param name="tileIDs"> The tileIDs to convert. </param>
        /// <param name="layer"> The final layer. </param>
        private void LoadIntoLayer(int[] tileIDs, MapLayer layer)
        {
            int currentID = 0;

            // Loops through all tiles in tileIDs, converts them to object of the type
            // Tile and loads them into the layer.
            for (int i = 0; i < layer.Height; i++)
            {
                for (int j = 0; j < layer.Width; j++)
                {
                    int id = tileIDs[currentID] - 1 < 0 ? 0 : tileIDs[currentID] - 1;
                    layer.SetTile(j, i, id, this.FindTileset(id));
                    currentID++;
                }
            }
        }

        /// <summary>
        /// Finds the tileset the tileID belongs to.
        /// </summary>
        /// <param name="tileID"> The tile id. </param>
        /// <returns> The number the tileset has in the list. </returns>
        /// <exception cref="Exception">
        /// An exception that means that the ID did not belong to any tileset.
        /// </exception>
        private int FindTileset(int tileID)
        {
            for (int i = 0; i < this.tilesetList.Count; i++)
            {
                if (this.tilesetList.ToArray()[i].FromThisTilset(tileID))
                {
                    return i;
                }
            }

            throw new Exception("ID not found in any tilesets.");
        }
        #endregion

        #region Virtual Method region
        #endregion
    }
}
