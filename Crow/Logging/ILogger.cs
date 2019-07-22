using System;

namespace Crow.Logging
{
    public interface ILogger
    {
        string Scope { get; set; }
        void Log(string loglevel, string content, string scope = "");
        void Log(string loglevel, object content, string scope = "");
    }
}