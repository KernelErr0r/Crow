using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Crow.Logging
{
    public class Logger : ILogger
    {
        public string Scope { get; set; }

        private object _lock = new object();
        private object _exceptionLock = new object();

        private int biggestLength = 0;

        private List<LogLevel> logLevels = new List<LogLevel>()
        {
            new LogLevel("Error", "[×]", Color.OrangeRed),
            new LogLevel("Warning", "[!]", Color.FromArgb(248, 250, 107)),
            new LogLevel("Success", "[v]", Color.LawnGreen),
            new LogLevel("Info", "[i]", Color.Cyan),
            new LogLevel("Stacktrace", "[o]", Color.FromArgb(248, 250, 107))
        };

        public Logger(string scope)
        {
            Scope = scope;

            foreach (var logLevel in logLevels)
            {
                if (biggestLength < logLevel.Name.Length)
                    biggestLength = logLevel.Name.Length;
            }
        }

        public void Log(string loglevel, string content, string scope = "")
        {
            lock(_lock)
            {
                string _scope = String.IsNullOrWhiteSpace(scope) ? Scope : scope;

                if (String.IsNullOrWhiteSpace(_scope))
                {
                    if (!String.IsNullOrWhiteSpace(loglevel))
                    {
                        foreach (var logLevel in logLevels)
                        {
                            if (logLevel.Name.ToLower() == loglevel.ToLower())
                            {
                                Console.WriteLine($"{logLevel.Icon.Pastel(logLevel.Color)} {ANSI.Underline(Extend(logLevel.Name, biggestLength)).Pastel(logLevel.Color)} {content.Pastel(Color.LightGray)}");
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{Extend("", biggestLength + 2)} {content.Pastel(Color.Gray)}");
                    }
                }
                else
                {

                    if (!String.IsNullOrWhiteSpace(loglevel))
                    {
                        foreach (var logLevel in logLevels)
                        {
                            if (logLevel.Name.ToLower() == loglevel.ToLower())
                            {
                                Console.WriteLine($"[{_scope}]".Pastel(Color.Gray) + $" {logLevel.Icon.Pastel(logLevel.Color)} { Extend(ANSI.Underline(logLevel.Name).Pastel(logLevel.Color), biggestLength) } {content.Pastel(Color.LightGray)}");
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[{_scope}]".Pastel(Color.Gray) + $" {Extend("", biggestLength + 2)} {content.Pastel(Color.LightGray)}");
                    }
                }
            }
        }

        public void Log(Exception exception, string scope = "")
        {
            lock (_exceptionLock)
            {
                var type = exception.GetType();
                List<string> lines = new List<string>(exception.StackTrace.Split(Environment.NewLine));

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

                Log("error", $"{type.Name.Pastel(Color.Gray)}: {exception.Message.Replace(Environment.NewLine, " ")}", scope);

                for (int i = 0; i < lines.Count; i++)
                {
                    if (i > 0)
                        Log("", "  " + lines[i], scope);
                    else
                        Log("stacktrace", lines[i], scope);
                };
            }
        }

        private string Extend(string str, int length)
        {
            int strLength = ANSI.Strip(str).Length;

            while (strLength < length)
            {
                str += ' ';
                strLength++;
            }

            return str;
        }
    }
}