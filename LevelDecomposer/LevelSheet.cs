using System;
using System.IO;
using Newtonsoft.Json;

namespace LevelDecomposer
{
    /// <summary>
    ///     Represents a level consisting of tiles.
    /// </summary>
    public class LevelSheet
    {
        /// <summary>
        ///     Create a <see cref="LevelSheet" /> from a JSON-serialized instance.
        /// </summary>
        /// <param name="path">File containing a JSON-serialized <see cref="LevelSheet" />.</param>
        /// <returns></returns>
        public static LevelSheet FromFileName(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            string text = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<LevelSheet>(text);
        }

        private readonly int _levelHeight;
        private readonly int _levelWidth;
        private readonly int _sheetHeight;
        private readonly string _sheetName;
        private readonly int _sheetWidth;
        private readonly int _tileHeight;
        private readonly int _tileWidth;
        private readonly int[] _tiles;

        // NOTE : Constructor is public for JSON serialization/deserialization.
#pragma warning disable 1591
        public LevelSheet(string sheetName, int sheetWidth, int sheetHeight, int levelWidth, int levelHeight,
#pragma warning restore 1591
            int tileWidth, int tileHeight, int[] tiles)
        {
            _sheetName = sheetName;
            _sheetWidth = sheetWidth;
            _sheetHeight = sheetHeight;
            _levelWidth = levelWidth;
            _levelHeight = levelHeight;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
            _tiles = tiles;
        }

        /// <summary>
        ///     Gets the file name of the associated tile sheet bitmap.
        /// </summary>
        public string SheetName
        {
            get { return _sheetName; }
        }

        /// <summary>
        ///     Gets the width in pixels of the associated tile sheet bitmap.
        /// </summary>
        public int SheetWidth
        {
            get { return _sheetWidth; }
        }

        /// <summary>
        ///     Gets the height in pixels of the associated tile sheet bitmap.
        /// </summary>
        public int SheetHeight
        {
            get { return _sheetHeight; }
        }

        /// <summary>
        ///     Gets the width in tiles of this level.
        /// </summary>
        public int LevelWidth
        {
            get { return _levelWidth; }
        }

        /// <summary>
        ///     Gets the height in tiles of this level.
        /// </summary>
        public int LevelHeight
        {
            get { return _levelHeight; }
        }

        /// <summary>
        ///     Gets the width in pixels of a tile.
        /// </summary>
        public int TileWidth
        {
            get { return _tileWidth; }
        }

        /// <summary>
        ///     Gets the height in pixels of a tile.
        /// </summary>
        public int TileHeight
        {
            get { return _tileHeight; }
        }

        /// <summary>
        ///     Gets the tiles of this level.
        /// </summary>
        public int[] Tiles
        {
            get { return _tiles; }
        }
    }
}