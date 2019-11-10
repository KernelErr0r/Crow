using System.IO;
using Crow.Api.Configs;
using Hjson;
using Newtonsoft.Json;

namespace Crow.Configs
{
    public class HjsonConfigDeserializer : IConfigDeserializer
    {
        public T DeserializeFromFile<T>(string file)
        {
            return Deserialize<T>(File.ReadAllText(file));
        }

        public T Deserialize<T>(string source)
        {
            var json = HjsonValue.Parse(source).ToString();

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}