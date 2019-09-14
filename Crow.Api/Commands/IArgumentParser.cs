using System.Collections.Generic;
using System.Reflection;

namespace Crow.Api.Commands
{
    public interface IArgumentParser
    {
        void RegisterTypeParser(object parser);
        void UnregisterTypeParser(object parser);
        
        List<object> Parse(MethodInfo methodInfo, params string[] arguments);
    }
}