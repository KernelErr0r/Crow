namespace Crow.Api.Plugins.Events
{
    public class PluginInitializedEvent : PluginEvent
    {
        public PluginInitializedEvent(Plugin plugin)
        {
            Plugin = plugin;
        }
    }
}