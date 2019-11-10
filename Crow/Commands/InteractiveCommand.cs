using System;
using System.Linq;
using Raven;

namespace Crow.Commands
{
    [Command("interactive", "interactive", "")]
    public class InteractiveCommand
    {
        private ICommandManager commandManager;
    
        public InteractiveCommand(ICommandManager commandManager)
        {
            this.commandManager = commandManager;
        }
        
        [Default]
        public void Default()
        {
            while (true)
            {
                var input = Console.ReadLine();
                var args = input?.Split(' ') ?? new string[0];

                if (args.Length > 0)
                {
                    commandManager.Invoke(args[0], args.Length == 1 ? new string[0] : args.Skip(1).ToArray());   
                }
            }
        }
    }
}