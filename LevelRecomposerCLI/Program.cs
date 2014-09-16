using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using LevelDecomposer;

namespace LevelRecomposerCLI
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
                    Level.Recompose(options.InputFile, options.OutputFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
