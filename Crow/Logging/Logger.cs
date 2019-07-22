using Crow.Extensions;
using Crow.Logging.Formatting;
using Crow.Logging.Formatting.Formatters;
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

        private int biggestLength = 0;

        private List<LogLevel> logLevels = new List<LogLevel>()
        {
            new LogLevel("Error", "[×]", Color.OrangeRed),
            new LogLevel("Warning", "[!]", Color.FromArgb(248, 250, 107)),
            new LogLevel("Success", "[v]", Color.LawnGreen),
            new LogLevel("Info", "[i]", Color.Cyan),
            new LogLevel("Stacktrace", "[o]", Color.FromArgb(248, 250, 107))
        };

        private List<IFormatter> formatters = new List<IFormatter>();

        public Logger(string scope)
        {
            Scope = scope;

            formatters.Add(new ExceptionFormatter(this));

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
                                Console.WriteLine($"{logLevel.Icon.Pastel(logLevel.Color)} {ANSI.Underline(logLevel.Name.Expand(biggestLength)).Pastel(logLevel.Color)} {content.Pastel(Color.LightGray)}");
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{"".Expand(biggestLength + 2)} {content.Pastel(Color.Gray)}");
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
                                Console.WriteLine($"[{_scope}]".Pastel(Color.Gray) + $" {logLevel.Icon.Pastel(logLevel.Color)} { ANSI.Underline(logLevel.Name).Pastel(logLevel.Color).Expand(biggestLength) } {content.Pastel(Color.LightGray)}");
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[{_scope}]".Pastel(Color.Gray) + $" {"".Expand(biggestLength + 2)} {content.Pastel(Color.LightGray)}");
                    }
                }
            }
        }

        public void Log(string loglevel, object input, string scope = "")
        {
            var inputType = input.GetType();

            foreach(var formatter in formatters)
            {
                var formatterType = formatter.GetType();
                var interfaces = formatterType.GetInterfaces();

                foreach(var interfac in interfaces)
                {
                    if(interfac.IsGenericType)
                    {
                        var genericArguments = interfac.GetGenericArguments();

                        foreach(var argument in genericArguments)
                        {
                            if(argument == inputType)
                            {
                                formatter.Format(input);

                                return;
                            }
                        }
                    }
                }
            }

            Log(loglevel, input.ToString(), scope);
        }
    }
}