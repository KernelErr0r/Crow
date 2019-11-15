namespace Crow.Api.Plugins.Events
{
    public class PluginPreInitializedEvent : PluginEvent
    {
        public PluginPreInitializedEvent(IPlugin plugin)
        {
            Plugin = plugin;
        }
    }
}