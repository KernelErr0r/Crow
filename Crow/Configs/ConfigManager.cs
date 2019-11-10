using System.Collections.Generic;
using Crow.Api.Configs;

namespace Crow.Configs
{
    public class ConfigManager : IConfigManager
    {
        private IConfigSerializer configSerializer;
        private IConfigDeserializer configDeserializer;

        public ConfigManager(IConfigSerializer configSerializer, IConfigDeserializer configDeserializer)
        {
            this.configSerializer = configSerializer;
            this.configDeserializer = configDeserializer;
        }

        private Dictionary<string, object> configs = new Dictionary<string, object>();
        
        public T Get<T>(string name)
        {
            return (T) configs[name];
        }

        public void Load<T>(string name, string file)
        {
            configs.Add(name, configDeserializer.DeserializeFromFile<T>(file));
        }
        
        public void Save<T>(string name, string file)
        {
            configSerializer.SerializeToFile(file, (T) configs[name]);
        }

        public void SaveDefault<T>(string file)
        {
            configSerializer.SerializeToFile(file, default(T));
        }

        public void Unload(string name)
        {
            configs.Remove(name);
        }
    }
}