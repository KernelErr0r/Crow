namespace Crow.Api.Plugins.Events
{
    public class PluginPreInitializedEvent : PluginEvent
    {
        public PluginPreInitializedEvent(Plugin plugin)
        {
            Plugin = plugin;
        }
    }
}