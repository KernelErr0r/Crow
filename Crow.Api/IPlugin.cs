namespace Crow.Api
{
    public interface IPlugin
    {
        string Name { get; }
        
        void Init();
        void LateInit();
    }
}