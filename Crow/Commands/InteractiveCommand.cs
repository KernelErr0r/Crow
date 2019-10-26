using System;
using System.Linq;
using Crow.Api;
using Raven;

namespace Crow.Commands
{
    [Command("interactive", "interactive", "")]
    public class InteractiveCommand
    {
        [Default]
        public void Default()
        {
            while (true)
            {
                var input = Console.ReadLine();
                var args = input?.Split(' ') ?? new string[0];

                if (args.Length > 0)
                {
                    CrowApi.CommandManager.Invoke(args[0], args.Length == 1 ? new string[0] : args.Skip(1).ToArray());   
                }
            }
        }
    }
}