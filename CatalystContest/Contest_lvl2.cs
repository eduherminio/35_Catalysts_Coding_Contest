using FileParser;
using System.Text;

namespace CatalystContest
{
    public class Contest_lvl2
    {
        private List<string> _input;
        private int _index;
        private StringBuilder _output;

        public void Run(string level)
        {
            _input = ParseInput(level).ToList();
            _index = 0;
            _output = new StringBuilder();

            var sb = new StringBuilder();

            Method(topLevelMethod: true);

            using var sw = new StreamWriter($"Output/{level}.out");

            sw.WriteLine(_output.ToString());
        }

        enum Mode { Print, Conditional, EndConditional };

        public string Method(bool topLevelMethod = false)
        {
            Mode mode = default;
            var conditionStack = new Stack<bool>();

            while (_index != _input.Count)
            {
                var item = _input[_index];
                switch (item)
                {
                    case "start":
                        ++_index;
                        var output = Method();
                        if (topLevelMethod)
                        {
                            return output;
                        }
                        break;
                    case "end":
                        switch (mode)
                        {
                            case Mode.Conditional:
                            case Mode.EndConditional:
                                mode = default;
                                break;
                            case Mode.Print:
                                break;
                            default: throw new();
                        }
                        break;
                    case "print":
                        _output.Append(_input[++_index]);
                        break;
                    case "if":
                        mode = Mode.Conditional;
                        break;
                    case "else":
                        mode = Mode.EndConditional;
                        while (_input[++_index] != "end") { }
                        break;
                    case "true":
                        conditionStack.Push(true);
                        break;
                    case "false":
                        while (_input[++_index] != "end") { }
                        if (_input[++_index] != "else") throw new();
                        //while (_input[++_index] != "end") { }
                        break;
                    case "return":
                        var outputValue = _input[++_index];
                        return outputValue;
                    default:
                        //switch (mode)
                        //{
                        //case Mode.Print:
                        //    sb.Append(item);
                        //    break;
                        //default:
                        //    break;
                        //throw new(); 
                        //case Mode.Conditional:
                        //    if (bool.TryParse(item, out var boo))
                        //        conditionStack.Push(boo);
                        //    else throw new();
                        //    break;
                        //case Mode.EndConditional: break;
                        //}
                        break;
                }
                ++_index;
            }

            return _output.ToString();
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
