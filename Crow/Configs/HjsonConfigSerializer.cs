using System.IO;
using Crow.Api.Configs;
using Hjson;
using Newtonsoft.Json;

namespace Crow.Configs
{
    public class HjsonConfigSerializer : IConfigSerializer
    {
        public void SerializeToFile<T>(string file, T source)
        {
            File.WriteAllText(file, Serialize(source));
        }

        public string Serialize<T>(T source)
        {
            var json = JsonConvert.SerializeObject(source, Formatting.Indented);
            var hjson = JsonValue.Parse(json).ToString(Stringify.Hjson);

            return hjson;
        }
    }
}