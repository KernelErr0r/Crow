using System;

namespace Crow.Logging
{
    public interface ILogger
    {
        void Log(string loglevel, string content, string scope = "");
        void Log(Exception exception, string scope = "");
    }
}
