using System;
using System.Collections.Generic;
using System.Reflection;
using Crow.Api.Commands;
using Crow.Commands.Parsers;
using Salem;

namespace Crow.Commands
{
    public class CommandManager : ICommandManager
    {
        public IReadOnlyDictionary<Command, object> Commands => commands;
        
        private Dictionary<Command, object> commands = new Dictionary<Command, object>();

        private ArgumentParser argumentParser = new ArgumentParser();
        private Logger logger = new Logger("Crow");

        public CommandManager()
        {
            argumentParser.RegisterTypeParser(new StringParser());
            argumentParser.RegisterTypeParser(new NumberParser());
        }

        public void Register(object command)
        {
            var type = command.GetType();
            var attribute = type.GetCustomAttribute(typeof(Command)) as Command;

            if (attribute != null)
            {
                commands.Add(attribute, command);
            }
            else
            {
                throw new ArgumentException("Invalid command");
            }
        }

        public void Unregister(object command)
        {
            var type = command.GetType();
            var attribute = type.GetCustomAttribute(typeof(Command)) as Command;

            if (attribute != null)
            {
                commands.Remove(attribute);
            }
            else
            {
                throw new ArgumentException("Invalid command");
            }
        }

        public void Invoke(string name, params string[] arguments)
        {
            try
            {
                foreach (var command in commands)
                {
                    if (command.Key.Name.ToLower() == name.ToLower())
                    {
                        var type = command.Value.GetType();
                        var invokeMethod = type.GetMethod("Invoke");
                        var parsedArgs = argumentParser.Parse(invokeMethod, arguments);
                        
                        invokeMethod.Invoke(command.Value, parsedArgs.ToArray());

                        return;
                    }
                }
            }
            catch (ArgumentException)
            {
                logger.Log("Error", "Incorrect usage");
            }
            
            logger.Log("Error", $"Command {name} doesn't exist");
        }
    }
}