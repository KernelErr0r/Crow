namespace Crow.Api.Plugins.Events
{
    public class PluginLoadedEvent : PluginEvent
    {
        public PluginLoadedEvent(Plugin plugin)
        {
            Plugin = plugin;
        }
    }
}