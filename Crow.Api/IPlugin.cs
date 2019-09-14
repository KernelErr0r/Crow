namespace Crow.Api
{
    public interface IPlugin
    {
        string Name { get; }
        
        bool PreInit();
        bool Init();
    }
}