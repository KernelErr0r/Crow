using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Crow.Logging.Formatting.Formatters
{
    public class ExceptionFormatter : IFormatter<Exception>
    {
        private object _exceptionLock = new object();
        private ILogger _logger;

        public ExceptionFormatter(ILogger logger)
        {
            _logger = logger;
        }

        public void Format(object input)
        {
            lock (_exceptionLock)
            {
                var exception = (Exception)input;

                var type = exception.GetType();

                _logger.Log("error", $"{type.Name.Pastel(Color.Gray)}: {exception.Message.Replace(Environment.NewLine, " ")}", _logger.Scope);

                if (exception.StackTrace != null)
                {
                    List<string> lines = new List<string>(exception.StackTrace.Contains(Environment.NewLine) ? exception.StackTrace.Split(Environment.NewLine) : new[] { exception.StackTrace });

                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (lines[i].StartsWith("   at "))
                        {
                            var path = lines[i].Substring(6, lines[i].IndexOf('(') - 6).Split('.');

                            for (int j = 0; j < path.Length; j++)
                                path[j] = path[j].Pastel(Color.Gray);

                            int openingBracketIndex = lines[i].IndexOf('(');
                            int closingBracketIndex = lines[i].IndexOf(')');

                            if (openingBracketIndex < closingBracketIndex - 1)
                            {
                                var args = lines[i].Substring(openingBracketIndex + 1, lines[i].Length - (openingBracketIndex + 2)).Split(',');

                                for (int j = 0; j < args.Length; j++)
                                {
                                    var args2 = args[j].Trim().Split(' ');

                                    args[j] = $"{args2[0].Pastel(Color.Gray)} {ANSI.Bold(args2[1])}";
                                }

                                lines[i] = "at ".Pastel(Color.Gray) + String.Join('.', path) + $"({String.Join(", ", args)})";
                            }
                            else
                            {
                                lines[i] = "at ".Pastel(Color.Gray) + String.Join('.', path) + lines[i].Substring(openingBracketIndex, lines[i].Length - openingBracketIndex);
                            }

                            if (lines[i].Contains(" in "))
                            {
                                int index = lines[i].IndexOf(" in ");
                                lines.Insert(i + 1, "    in".Pastel(Color.Gray) + lines[i].Substring(index, lines[i].Length - (index)).Substring(3));

                                int lineIndex = lines[i + 1].IndexOf("line ");
                                lines.Insert(i + 2, "        at ".Pastel(Color.Gray) + lines[i + 1].Substring(lineIndex));
                                lines[i + 1] = lines[i + 1].Substring(0, lineIndex - 1);
                                lines[i] = lines[i].Substring(0, index);

                                i += 2;
                            }
                        }
                    }

                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (i > 0)
                            _logger.Log("", "  " + lines[i], _logger.Scope);
                        else
                            _logger.Log("stacktrace", lines[i], _logger.Scope);
                    };
                }
            }
        }
    }
}