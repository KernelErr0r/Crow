namespace Crow.Api.Plugins
{
    public class Plugin
    {
        public IPlugin Instance { get; }
        public PluginAttribute Attribute { get; }

        public Plugin(IPlugin instance, PluginAttribute attribute)
        {
            Instance = instance;
            Attribute = attribute;
        }
    }
}