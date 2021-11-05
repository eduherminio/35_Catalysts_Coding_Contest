using FileParser;

namespace CatalystContest
{
    public class Contest_lvl1
    {
        public void Run(string level)
        {
            var input = ParseInput(level).ToList();

            using var sw = new StreamWriter($"Output/{level}.out");

            sw.WriteLine($"{input.Min()}");
        }

        private IEnumerable<int> ParseInput(string level)
        {
            var file = new ParsedFile($"Inputs/{level}");

            var numberOfLines = file.NextLine().NextElement<int>();
            for (int i = 0; i < numberOfLines; ++i)
            {
                var line = file.NextLine();
                yield return line.NextElement<int>();
            }
            if (!file.Empty)
            {
                throw new ParsingException("Error parsing file");
            }
        }
    }
}
