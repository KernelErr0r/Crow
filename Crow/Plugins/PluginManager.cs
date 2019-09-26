using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Crow.Api.Plugins;
using Salem;

namespace Crow.Plugins
{
    public class PluginManager
    {
        private readonly ConcurrentBag<IPlugin> plugins = new ConcurrentBag<IPlugin>();
        private readonly Logger logger = new Logger("PluginManager");
        
        public void LoadPlugins()
        {
            var files = new ConcurrentBag<string>(Directory.GetFiles(Crow.Instance.PluginsDirectory));

            Parallel.ForEach(files, (file) =>
            {
                if (Path.GetExtension(file) == ".dll")
                {
                    var rawAssembly = File.ReadAllBytes(file);
                    var assembly = AppDomain.CurrentDomain.Load(rawAssembly);

                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.GetInterface("IPlugin") != null)
                        {
                            var plugin = Activator.CreateInstance(type) as IPlugin;
                        
                            plugins.Add(plugin);
                        }
                    }   
                }
            });
        }
        
        public void InitializePlugins()
        {
            Parallel.ForEach(plugins, async (plugin) =>
            {
                await logger.Log(plugin.PreInit, $"Preinitializing {plugin.Name}", $"Successfully preinitialized {plugin.Name}", $"Couldn't preinitialize {plugin.Name}"); 
            });

            
            Parallel.ForEach(plugins, async (plugin) =>
            {
                await logger.Log(plugin.Init, $"Initializing {plugin.Name}", $"Successfully initialized {plugin.Name}", $"Couldn't initialize {plugin.Name}");
            });
        }
    }
}