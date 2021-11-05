using FileParser;
using System.Text;

namespace CatalystContest
{
    public class Contest_lvl1
    {
        public void Run(string level)
        {
            var input = ParseInput(level).ToList();

            var sb = new StringBuilder();

            foreach (var item in input)
            {
                switch (item)
                {
                    case "print":
                    case "start":
                    case "end":
                        break;
                    default: sb.Append(item); break;
                }
            }

            using var sw = new StreamWriter($"Output/{level}.out");

            sw.WriteLine(sb.ToString());
        }

        private IEnumerable<string> ParseInput(string level)
        {
            var file = new ParsedFile($"Inputs/{level}");
            var numberOfLines = file.NextLine().NextElement<int>();
            for (int i = 0; i < numberOfLines; ++i)
            {
                var line = file.NextLine();
                while (!line.Empty)
                {
                    yield return line.NextElement<string>();
                }
            }
            if (!file.Empty)
            {
                throw new ParsingException("Error parsing file");
            }
        }
    }
}
