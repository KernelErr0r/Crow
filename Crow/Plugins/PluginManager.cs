using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Crow.Api.Plugins;
using Crow.Api.Plugins.Events;
using Redbus.Interfaces;
using Salem;

namespace Crow.Plugins
{
    public class PluginManager : IPluginManager
    {
        public IReadOnlyList<Plugin> Plugins => plugins.ToArray();

        private readonly ConcurrentBag<Plugin> plugins = new ConcurrentBag<Plugin>();
        private readonly Logger logger = new Logger("PluginManager");
        
        private IEventBus eventBus;

        public PluginManager(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }
        
        public void LoadPlugins()
        {
            var files = new ConcurrentBag<string>(Directory.GetFiles(Crow.Instance.PluginsDirectory));

            Parallel.ForEach(files, (file) =>
            {
                if (Path.GetExtension(file) == ".dll")
                {
                    var rawAssembly = File.ReadAllBytes(file);
                    var assembly = AppDomain.CurrentDomain.Load(rawAssembly);

                    var pluginTypes = assembly.GetTypes().Where(type => type.GetCustomAttribute<PluginAttribute>() is { });

                    if (pluginTypes.Count() == 1)
                    {
                        var pluginType = pluginTypes.First();
                        var attribute = pluginType.GetCustomAttribute<PluginAttribute>();

                        LoadPluginInternal(pluginType, attribute);
                    }
                    else
                    {
                        throw new InvalidPluginException();
                    }
                }
            });
        }

        public void LoadPlugin(object obj)
        {
            var type = obj.GetType();

            if (type.GetCustomAttribute<PluginAttribute>() is { } attribute)
            {
                LoadPluginInternal(type, attribute);
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
                logger.Log("Info", $"Preinitializing {plugin.Attribute.Name}");
                
                if (plugin.Instance.PreInit())
                {
                    logger.Log("Success", $"Successfully preinitialized {plugin.Attribute.Name}");
                }
                else
                {
                    logger.Log("Error", $"Couldn't preinitialize {plugin.Attribute.Name}");
                }
                
                eventBus.Publish(new PluginPreInitializedEvent(plugin));
            });
        }
        
        public void InitializePlugins()
        {
            Parallel.ForEach(plugins, async (plugin) =>
            {
                logger.Log("Info", $"Initializing {plugin.Attribute.Name}");
                
                if (plugin.Instance.Init())
                {
                    logger.Log("Success", $"Successfully initialized {plugin.Attribute.Name}");
                }
                else
                {
                    logger.Log("Error", $"Couldn't initialize {plugin.Attribute.Name}");
                }
                
                eventBus.Publish(new PluginInitializedEvent(plugin));
            });
        }

        private void LoadPluginInternal(Type type, PluginAttribute attribute)
        {
            var instance = Activator.CreateInstance(type) as IPlugin;
            var plugin = new Plugin(instance, attribute);

            plugins.Add(plugin);
            eventBus.SubscribeAll(type, plugin);
            eventBus.Publish(new PluginLoadedEvent(plugin));
        }
    }
}