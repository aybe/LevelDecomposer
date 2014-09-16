using CommandLine;
using CommandLine.Text;

namespace LevelDecomposerCLI
{
    internal class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input PNG path")]
        public string InputFile { get; set; }

        [Option('w', "width", Required = true,
            HelpText = "Tile width, must be a multiple of input width")]
        public int TileWidth { get; set; }

        [Option('h', "height", Required = true,
            HelpText = "Tile height, must be a multiple of input height")]
        public int TileHeight { get; set; }

        [Option('s', "sheet", Required = true, HelpText = "Output PNG path")]
        public string OutputImage { get; set; }

        [Option('j', "json", Required = true, HelpText = "Output JSON path")]
        public string OutputJson { get; set; }

        [Option('l', "length", Required = true, HelpText = "Tile sheet width desired, in pixels")]
        public int SheetWidth { get; set; }


        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, s => HelpText.DefaultParsingErrorsHandler(this, s));
        }
    }
}