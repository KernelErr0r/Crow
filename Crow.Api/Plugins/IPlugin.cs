namespace Crow.Api.Plugins
{
    public interface IPlugin
    {
        bool PreInit();
        bool Init();
    }
}