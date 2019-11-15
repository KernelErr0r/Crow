using Crow.Plugins;
using Redbus;
using Xunit;

namespace Crow.Tests
{
    public class PluginManagerTests
    {
        [Fact]
        public void TestLoading()
        {
            var pluginManager = new PluginManager(new EventBus());
            var testPlugin = new TestPlugin();
            
            pluginManager.LoadPlugin(testPlugin);
            
            Assert.Single(pluginManager.Plugins);
        }

        [Fact]
        public void TestInitialization()
        {
            var pluginManager = new PluginManager(new EventBus());
            var testPlugin = new TestPlugin();
            
            pluginManager.LoadPlugin(testPlugin);
            pluginManager.InitializePlugins();

            var loadedPlugin = pluginManager.Plugins[0] as TestPlugin;
            
            Assert.True(loadedPlugin.Initialized);
        }
        
        [Fact]
        public void TestPreInitialization()
        {
            var pluginManager = new PluginManager(new EventBus());
            var testPlugin = new TestPlugin();
            
            pluginManager.LoadPlugin(testPlugin);
            pluginManager.PreInitializePlugins();

            var loadedPlugin = pluginManager.Plugins[0] as TestPlugin;
            
            Assert.True(loadedPlugin.PreInitialized);
        }
    }
}