using Redbus.Events;

namespace Crow.Api.Plugins.Events
{
    public class PluginEvent : EventBase
    {
        public Plugin Plugin { get; protected set; }
    }
}