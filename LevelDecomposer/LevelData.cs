using System;
using System.Collections.Generic;

namespace LevelDecomposer
{
    internal class LevelData
    {
        public readonly byte[] Pixels;

        public readonly List<Tuple<int, int>> Tiles;

        public LevelData(byte[] pixels)
        {
            Pixels = pixels;
            Tiles = new List<Tuple<int, int>>();
        }
    }
}