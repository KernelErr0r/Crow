namespace Crow.Api.Configs
{
    public interface IConfigManager
    {
        T Get<T>(string name);
        void Load<T>(string name, string file);
        void Save<T>(string name, string file);
        void SaveDefault<T>(string file);
        void Unload(string name);
    }
}