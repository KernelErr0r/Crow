using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Crow.Api.Plugins;
using Salem;

namespace Crow.Plugins
{
    public class PluginManager
    {
        public IReadOnlyList<IPlugin> Plugins => plugins.ToArray();

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

                            return;
                        }
                    }

                    throw new InvalidPluginException();
                }
            });
        }

        public void LoadPlugin(object obj)
        {
            var type = obj.GetType();
            
            if (type.GetInterface("IPlugin") != null)
            {
                var plugin = Activator.CreateInstance(type) as IPlugin;
                        
                plugins.Add(plugin);
            }
            else
            {
                throw new InvalidPluginException();
            }
        }

        public void PreInitializePlugins()
        {
            Parallel.ForEach(plugins, async (plugin) =>
            {
                logger.Log("Info", $"Preinitializing {plugin.Name}");
                
                if (plugin.PreInit())
                {
                    logger.Log("Success", $"Successfully preinitialized {plugin.Name}");
                }
                else
                {
                    logger.Log("Error", $"Couldn't preinitialize {plugin.Name}");
                }
            });
        }
        
        public void InitializePlugins()
        {
            Parallel.ForEach(plugins, async (plugin) =>
            {
                logger.Log("Info", $"Initializing {plugin.Name}");
                
                if (plugin.Init())
                {
                    logger.Log("Success", $"Successfully initialized {plugin.Name}");
                }
                else
                {
                    logger.Log("Error", $"Couldn't initialize {plugin.Name}");
                }
            });
        }
    }
}