using System.Collections.Generic;

namespace Crow.Api.Plugins
{
    public interface IPluginManager
    {
        IReadOnlyList<IPlugin> Plugins { get; }
    
        void LoadPlugins();
        void InitializePlugins();
    }
}