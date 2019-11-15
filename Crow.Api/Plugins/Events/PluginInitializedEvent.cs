namespace Crow.Api.Plugins.Events
{
    public class PluginInitializedEvent : PluginEvent
    {
        public PluginInitializedEvent(IPlugin plugin)
        {
            Plugin = plugin;
        }
    }
}