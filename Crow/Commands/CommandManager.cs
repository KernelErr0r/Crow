using System;
using System.Collections.Generic;
using Crow.Api.Commands;

namespace Crow.Commands
{
    public class CommandManager
    {
        public IReadOnlyList<ICommand> Commands => commands;

        private List<ICommand> commands = new List<ICommand>();

        public void Register(ICommand command) =>
            commands.Add(command);

        public void Unregister(ICommand command) =>
            commands.Remove(command);

        public void Invoke(string name, params string[] arguments)
        {
            foreach (var command in commands)
            {
                if (command.Name.ToLower() == name.ToLower())
                {
                    command.Invoke(arguments);

                    return;
                }
            }

            throw new Exception($"Command {name} doesn't exist");
        }
    }
}