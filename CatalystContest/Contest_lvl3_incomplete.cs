using FileParser;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using System.Text;

namespace CatalystContest
{
    public class Contest_lvl3_incomplete
    {
        private List<string> _input;
        private int _index;
        //private StringBuilder _output;

        public void Run(string level)
        {
            _input = ParseInput(level).ToList();
            _index = 0;
            //_output = new StringBuilder();

            using (var sw = new StreamWriter($"Output/{level}.out")) { }
            var output = new StringBuilder();
            Method(level, output, topLevelMethod: true);

            using (var sw = new StreamWriter($"Output/{level}.out", append: true))
            {
                sw.WriteLine(output.ToString());
            }
        }

        enum Mode { Print, Conditional, EndConditional };

        public string Method(string level, StringBuilder _output, bool topLevelMethod = false)
        {
            //var _output = new StringBuilder();

            Mode mode = default;
            var conditionStack = new Stack<bool>();
            var elseStack = new Stack<bool>();
            Dictionary<string, string> variables = new();

            while (_index < _input.Count)
            {
                var item = _input[_index];
                if (conditionStack.TryPeek(out var cond) && !cond && item != "end")
                {
                    ++_index;
                    continue;
                }
                switch (item)
                {
                    case "start":
                        ++_index;

                        try
                        {
                            Method(level, _output);
                            _output.Append(Environment.NewLine);
                            //using (var sw = new StreamWriter($"Output/{level}.out", append: true))
                            //{
                            //    sw.WriteLine(_output.ToString());
                            //}

                            //_output.Clear();


                            //if (topLevelMethod)
                            //{
                            //    if (_output.Length > 0)
                            //    {
                            //using (var sw = new StreamWriter($"Output/{level}.out", append: true))
                            //{
                            //    sw.WriteLine(_output.ToString());
                            //}
                            //_output.Clear();
                            //}

                            //topLevelMethod = false;
                            //while (++_index < _input.Count && _input[_index] != "start") { }
                            //--_index;
                            //}
                            //else
                            //{
                            //    return methodOutput;
                            //}
                        }
                        catch (CustomException)
                        {
                            if (topLevelMethod)
                            {
                                topLevelMethod = false;
                            }
                            using (var sw = new StreamWriter($"Output/{level}.out", append: true))
                            {
                                sw.WriteLine("ERROR");
                            }
                            _output.Clear();
                            while (++_index < _input.Count && _input[_index] != "start") { }
                            --_index;
                        }
                        break;
                    case "end":

                        bool noCondition = conditionStack.Count != 0;
                        if (noCondition && conditionStack.Pop())
                        {
                            conditionStack.Push(false);
                        }

                        if (!noCondition)
                        {
                            if (elseStack.Count > 0)
                            {
                                elseStack.Pop();
                            }
                            else
                            {
                                using (var sw = new StreamWriter($"Output/{level}.out", append: true))
                                {
                                    sw.WriteLine(_output.ToString());
                                    _output.Clear();
                                }
                            }
                        }
                        //switch (mode)
                        //{
                        //    case Mode.Conditional:
                        //        conditionStack.Pop();
                        //        break;
                        //    case Mode.EndConditional:
                        //        mode = default;
                        //        break;
                        //    case Mode.Print:
                        //        break;
                        //    default: throw new();
                        //}
                        break;
                    case "print":
                        var value = _input[++_index];
                        if (!variables.TryGetValue(value, out var str))
                        {
                            str = value;
                        }
                        _output.Append(str);
                        break;
                    case "if":
                        mode = Mode.Conditional;
                        break;
                    case "else":
                        //mode = Mode.EndConditional;
                        //while (_input[++_index] != "end") { }
                        break;
                    case "true":
                        conditionStack.Push(true);
                        break;
                    case "false":
                        conditionStack.Push(false);
                        //while (_input[++_index] != "end") { }
                        //if (_input[++_index] != "else") throw new();
                        //while (_input[++_index] != "end") { }
                        break;
                    case "return":
                        var returnValue = _input[++_index];
                        while (conditionStack.Count > 0)
                        {
                            if (_input[++_index] == "end")
                            {
                                conditionStack.Pop();
                            }
                        }
                        while (_input[++_index] != "start") { }
                        --_index;
                        return returnValue;
                    case "set":
                        {
                            var name = _input[++_index];
                            if (!variables.ContainsKey(name))
                            {
                                throw new CustomException();
                            }

                            var originalValue = _input[++_index];

                            while (variables.TryGetValue(originalValue, out var tmpValue)) { originalValue = tmpValue; }

                            variables[name] = originalValue;
                            break;
                        }
                    case "var":
                        {
                            var name = _input[++_index];
                            if (variables.ContainsKey(name))
                            {
                                throw new CustomException();
                            }

                            var originalValue = _input[++_index];

                            while (variables.TryGetValue(originalValue, out var tmpValue)) { originalValue = tmpValue; }

                            variables[name] = originalValue;
                            break;
                        }
                    default:
                        switch (mode)
                        {
                            case Mode.Conditional:
                                if (!bool.TryParse(item, out var boo) && variables.TryGetValue(item, out var stringBoo) && !bool.TryParse(stringBoo, out boo))
                                {
                                    throw new CustomException();
                                }
                                if (boo)
                                {
                                    conditionStack.Push(boo);
                                }
                                else
                                {
                                    while (_input[++_index] != "end") { }
                                    if (_input[++_index] != "else") throw new();
                                    elseStack.Push(true);
                                }
                                break;
                            default:
                                break;
                            //case Mode.Print:
                            //    sb.Append(item);
                            //    break;
                            //default:
                            //    break;
                            //throw new(); 
                            case Mode.EndConditional: break;
                        }
                        break;
                }
                ++_index;
            }

            return "";
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

    [Serializable]
    internal class CustomException : Exception
    {
        public CustomException()
        {
        }

        public CustomException(string? message) : base(message)
        {
        }

        public CustomException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected CustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
