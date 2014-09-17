using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace LevelDecomposer
{
    /// <summary>
    /// Provides methods for converting a bitmap level to a tile sheet / level data and vice-versa.
    /// </summary>
    public static class Level
    {

        /// <summary>
        /// Decomposes a level bitmap to a tile sheet and a <see cref="LevelSheet"/>.
        /// </summary>
        /// <param name="fileName">Bitmap of the level to decompose.</param>
        /// <param name="tileWidth">Tile width, must be a multiple of <see cref="fileName"/> width.</param>
        /// <param name="tileHeight">Tile height, must be a multiple of <see cref="fileName"/> height.</param>
        /// <param name="targetJson">File to write the <see cref="LevelSheet"/> to.</param>
        /// <param name="targetPng">File to write the level tiles to.</param>
        /// <param name="sheetWidth">Width in pixels of the tile sheet, must be a multiple of 2.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void Decompose(string fileName, int tileWidth, int tileHeight, string targetJson, string targetPng, int sheetWidth)
        {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName))
                throw new ArgumentOutOfRangeException("fileName", "Input file name does not exist");

            if (tileWidth <= 0) throw new ArgumentOutOfRangeException("tileWidth");
            if (tileHeight <= 0) throw new ArgumentOutOfRangeException("tileHeight");
            var image = new BitmapImage(new Uri(fileName, UriKind.RelativeOrAbsolute));
            WriteableBitmap bitmap = BitmapFactory.ConvertToPbgra32Format(image);
            int pixelWidth = bitmap.PixelWidth;
            int pixelHeight = bitmap.PixelHeight;
            if (pixelWidth % tileWidth != 0)
                throw new ArgumentOutOfRangeException("tileWidth", "Tile width must be a multiple of image width");
            if (pixelHeight % tileHeight != 0)
                throw new ArgumentOutOfRangeException("tileHeight", "Tile height must be a multiple of image height");

            if (targetJson == null) throw new ArgumentNullException("targetJson");
            if (targetPng == null) throw new ArgumentNullException("targetPng");

            if (sheetWidth <= 0)
                throw new ArgumentOutOfRangeException("sheetWidth", "Must a multiple of two");
            if (sheetWidth %2 !=0)
                throw new ArgumentOutOfRangeException("sheetWidth", "Must a multiple of two");
            int hTiles = pixelWidth / tileWidth;
            int vTiles = pixelHeight / tileHeight;
            WriteableBitmap target = BitmapFactory.New(tileWidth, tileHeight);
            var dictionary = new Dictionary<string, LevelData>();
            for (int y = 0; y < vTiles; y++)
            {
                for (int x = 0; x < hTiles; x++)
                {
                    target.Blit(new Rect(0, 0, tileWidth, tileHeight), bitmap,
                        new Rect(x * tileWidth, y * tileHeight, tileWidth, tileHeight));

                    var bmpBitmapEncoder = new BmpBitmapEncoder();
                    bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(target));
                    byte[] buffer;
                    using (var memoryStream = new MemoryStream())
                    {
                        bmpBitmapEncoder.Save(memoryStream);
                        buffer = memoryStream.GetBuffer();
                    }

                    byte[] hash;
                    using (SHA1 sha1 = SHA1.Create())
                    {
                        hash = sha1.ComputeHash(buffer);
                    }
                    IEnumerable<string> @select = hash.Select(s => s.ToString("X"));
                    string key = String.Concat(@select);

                    bool containsKey = dictionary.ContainsKey(key);
                    if (!containsKey)
                    {
                        dictionary.Add(key, new LevelData(buffer));
                    }
                    dictionary[key].Tiles.Add(new Tuple<int, int>(x, y));
                }
            }

            // generate sheet
            int count = dictionary.Count;
            int tilesPerRow = sheetWidth / tileWidth;
            int rows = count / tilesPerRow + 1;

            int desiredHeight = rows * tileHeight;
            WriteableBitmap writeableBitmap = BitmapFactory.New(tilesPerRow * tileWidth, rows * tileHeight);
            int i = 0;
            foreach (var myClass in dictionary)
            {
                byte[] bytes = myClass.Value.Pixels;
                BmpBitmapDecoder decoder;
                using (var memoryStream = new MemoryStream(bytes))
                {
                    decoder = new BmpBitmapDecoder(memoryStream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                }
                BitmapFrame bitmapFrame = decoder.Frames[0];
                WriteableBitmap source = BitmapFactory.ConvertToPbgra32Format(bitmapFrame);
                var destRect = new Rect(i * tileWidth, 0, tileWidth, tileHeight);
                destRect.X = i % tilesPerRow * tileWidth;
                destRect.Y = i / tilesPerRow * tileHeight;

                writeableBitmap.Blit(destRect, source, new Rect(0, 0, tileWidth, tileHeight));
                i++;
            }


            // save level
            int i1 = hTiles * vTiles;
            var level = new int[i1];
            int j = 0;
            foreach (var myClass in dictionary)
            {
                List<Tuple<int, int>> tuples = myClass.Value.Tiles;
                foreach (var tuple in tuples)
                {
                    int x = tuple.Item1;
                    int y = tuple.Item2;
                    int offset = y * hTiles + x;
                    level[offset] = j;
                }
                j++;
            }


            // Save bitmap
            string pngDirectory = EnsureDirectoryExists(targetPng);
            string pngFile = Path.GetFileName(targetPng);
            string pngPath = Path.Combine(pngDirectory, pngFile);
            using (Stream stream = File.OpenWrite(pngPath))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(writeableBitmap));
                encoder.Save(stream);
            }

            // Save sheet
            var sheet = new LevelSheet(Path.GetFileName(targetPng), sheetWidth, desiredHeight, hTiles, vTiles,
                tileWidth, tileHeight, level);
            string json = JsonConvert.SerializeObject(sheet, Formatting.Indented);
            string jsonDirectory = EnsureDirectoryExists(targetJson);
            string jsonFile = Path.GetFileName(targetJson);
            string jsonPath = Path.Combine(jsonDirectory, jsonFile);
            File.WriteAllText(jsonPath, json);
        }

        /// <summary>
        ///     Ensures that the directory of the specified file name exists, if no directory could be inferred, current directory
        ///     is used.
        /// </summary>
        /// <param name="fileName">File name for which to ensure directory exists.</param>
        /// <returns>The directory found/created.</returns>
        private static string EnsureDirectoryExists(string fileName)
        {
            // Get/create target directory
            string directoryName = Path.GetDirectoryName(fileName);
            if (String.IsNullOrEmpty(directoryName))
            {
                directoryName = Environment.CurrentDirectory;
            }
            else
            {
                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);
            }
            return directoryName;
        }

        /// <summary>
        /// Recomposes a <see cref="LevelSheet"/> to a level bitmap.
        /// </summary>
        /// <param name="inputFile">Input file containing a JSON-serialized <see cref="LevelSheet"/>.</param>
        /// <param name="outputFile">Output file of the bitmap generated.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static void Recompose(string inputFile, string outputFile)
        {
            if (inputFile == null) throw new ArgumentNullException("inputFile");
            if (outputFile == null) throw new ArgumentNullException("outputFile");
            if (!File.Exists(inputFile))
                throw new ArgumentOutOfRangeException("inputFile", "Input file does not exist");

            LevelSheet sheet = LevelSheet.FromFileName(inputFile);
            string sheetName = sheet.SheetName;
            
            var directory = EnsureDirectoryExists(inputFile);
            var uriString = Path.Combine(directory, sheetName);
            var bitmapImage = new BitmapImage(new Uri(uriString, UriKind.RelativeOrAbsolute));
            WriteableBitmap source = BitmapFactory.ConvertToPbgra32Format(bitmapImage);

            int levelWidth = sheet.LevelWidth;
            int levelHeight = sheet.LevelHeight;
            int tileWidth = sheet.TileWidth;
            int tileHeight = sheet.TileHeight;
            WriteableBitmap target = BitmapFactory.New(levelWidth * tileWidth, levelHeight * tileHeight);
            for (int y = 0; y < levelHeight; y++)
            {
                for (int x = 0; x < levelWidth; x++)
                {
                    int i = sheet.Tiles[y * levelWidth + x];
                    var sourceRect = new Rect();
                    int tilesPerRow = sheet.SheetWidth / tileWidth;
                    sourceRect.X = i % tilesPerRow * tileWidth;
                    sourceRect.Y = i / tilesPerRow * tileHeight;
                    sourceRect.Width = tileWidth;
                    sourceRect.Height = tileHeight;
                    target.Blit(new Rect(x * tileWidth, y * tileHeight, tileWidth, tileHeight), source, sourceRect);
                }
            }

            string outputDirectory = EnsureDirectoryExists(outputFile);
            string outputPath = Path.Combine(outputDirectory, Path.GetFileName(outputFile));
            using (FileStream fileStream = File.OpenWrite(outputPath))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(target));
                encoder.Save(fileStream);
            }
        }
    }
}