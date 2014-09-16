using CommandLine;
using CommandLine.Text;

namespace LevelRecomposerCLI
{
    internal class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input JSON level path")]
        public string InputFile { get; set; }


        [Option('o', "output", Required = true, HelpText = "Output image path")]
        public string OutputFile { get; set; }


        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, s => HelpText.DefaultParsingErrorsHandler(this, s));
        }
    }
}