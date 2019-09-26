namespace Crow.Api.Plugins
{
    public interface IPlugin
    {
        string Name { get; }
        
        bool PreInit();
        bool Init();
    }
}