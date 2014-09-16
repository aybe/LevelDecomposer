using System.IO;
using Newtonsoft.Json;

namespace LevelDecomposer
{
    public class LevelSheet
    {

        public static LevelSheet FromFileName(string path)
        {
            var text = File.ReadAllText(path);
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

        public LevelSheet(string sheetName, int sheetWidth, int sheetHeight, int levelWidth, int levelHeight,
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

        public string SheetName
        {
            get { return _sheetName; }
        }

        public int SheetWidth
        {
            get { return _sheetWidth; }
        }

        public int SheetHeight
        {
            get { return _sheetHeight; }
        }

        public int LevelWidth
        {
            get { return _levelWidth; }
        }

        public int LevelHeight
        {
            get { return _levelHeight; }
        }

        public int TileWidth
        {
            get { return _tileWidth; }
        }

        public int TileHeight
        {
            get { return _tileHeight; }
        }

        public int[] Tiles
        {
            get { return _tiles; }
        }
    }
}