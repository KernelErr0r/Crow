namespace Crow.Api.Plugins.Events
{
    public class PluginLoadedEvent : PluginEvent
    {
        public PluginLoadedEvent(IPlugin plugin)
        {
            Plugin = plugin;
        }
    }
}