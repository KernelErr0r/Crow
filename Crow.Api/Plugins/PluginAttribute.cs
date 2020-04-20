using System;

namespace Crow.Api.Plugins
{
    public class PluginAttribute : Attribute
    {
        public string Name { get; }
        public string Description { get; }

        public PluginAttribute(string name, string description = "")
        {
            Name = name;
            Description = description;
        }
    }
}