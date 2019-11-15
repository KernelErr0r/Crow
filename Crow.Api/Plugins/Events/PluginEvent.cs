using Redbus.Events;

namespace Crow.Api.Plugins.Events
{
    public class PluginEvent : EventBase
    {
        public IPlugin Plugin { get; protected set; }
    }
}