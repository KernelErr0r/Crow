namespace Crow.Api.Plugins
{
    public interface IPluginManager
    {
        void LoadPlugins();
        void InitializePlugins();
    }
}