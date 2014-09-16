using System;
using CommandLine;
using LevelDecomposer;

namespace LevelDecomposerCLI
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // note : see CLI debug args
            try
            {
                var options = new Options();
                if (Parser.Default.ParseArguments(args, options))
                {
                    Decompose(options);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void Decompose(Options options)
        {
            Level.Decompose(options.InputFile, options.TileWidth, options.TileHeight, options.OutputJson,
                options.OutputImage, options.SheetWidth);
        }
    }
}