using Crow.Api.Plugins;

namespace Crow.Tests
{
    public class TestPlugin : IPlugin
    {
        public string Name { get; } = "Test";

        public bool PreInitialized { get; private set; }
        public bool Initialized { get; private set; }
        
        public bool PreInit()
        {
            PreInitialized = true;

            return true;
        }

        public bool Init()
        {
            Initialized = true;

            return true;
        }
    }
}