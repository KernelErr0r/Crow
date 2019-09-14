namespace Crow.Api
{
    public interface IPlugin
    {
        void Init();
        void LateInit();
    }
}