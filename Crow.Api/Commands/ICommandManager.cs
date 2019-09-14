using System.Collections.Generic;

namespace Crow.Api.Commands
{
    public interface ICommandManager
    {
        IReadOnlyList<ICommand> Commands { get; }
        
        void Register(ICommand command);
        void Unregister(ICommand command);
        void Invoke(string name, params string[] arguments);
    }
}