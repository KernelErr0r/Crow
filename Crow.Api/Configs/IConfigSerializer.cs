namespace Crow.Api.Configs
{
    public interface IConfigSerializer
    {
        void SerializeToFile<T>(string file, T source);
        string Serialize<T>(T source);
    }
}