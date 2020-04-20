using System.Collections.Generic;

namespace Crow.Api.Plugins
{
    public interface IPluginManager
    {
        IReadOnlyList<Plugin> Plugins { get; }
    
        void LoadPlugins();
        void LoadPlugin(object obj);
        void InitializePlugins();
        void PreInitializePlugins();
    }
}