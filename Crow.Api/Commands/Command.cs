using System;

namespace Crow.Api.Commands
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public class Command : Attribute
    {
        public string Name { get; }
        public string Usage { get; }
        public string Description { get; }

        public Command(string name, string usage, string description)
        {
            Name = name;
            Usage = usage;
            Description = description;
        }
    }
}