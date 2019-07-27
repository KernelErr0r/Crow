using Crow.Logging.Outputs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crow.Logging
{
    public interface ILogger
    {
        string Scope { get; set; }
        List<IOutput> Outputs { get; }
        void Log(string logLevel, string content, string scope = "");
        void Log(string logLevel, object content, string scope = "");
        Task Log(Func<bool> task, string message, string successMessage, string failMessage, string logLevel = "Awaiting", string successLoglevel = "Success", string failLogLevel = "Error", string scope = "");
    }
}