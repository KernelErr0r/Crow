namespace Crow.Api.Configs
{
    public interface IConfigDeserializer
    {
        T DeserializeFromFile<T>(string file);
        T Deserialize<T>(string source);
    }
}