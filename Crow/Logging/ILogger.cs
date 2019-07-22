using Crow.Logging.Outputs;
using System.Collections.Generic;

namespace Crow.Logging
{
    public interface ILogger
    {
        string Scope { get; set; }
        List<IOutput> Outputs { get; }
        void Log(string loglevel, string content, string scope = "");
        void Log(string loglevel, object content, string scope = "");
    }
}