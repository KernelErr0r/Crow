using System.Collections.Generic;

namespace Crow.Api.Commands
{
    public interface ICommandManager
    {
        IReadOnlyDictionary<Command, object> Commands { get; }
        
        void Register(object command);
        void Unregister(object command);

        void Invoke(string name, params string[] arguments);
    }
}