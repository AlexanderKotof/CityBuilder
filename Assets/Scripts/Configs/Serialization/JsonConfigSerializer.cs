using System;
using Configs.Converter;
using Configs.Schemes;
using Newtonsoft.Json;

namespace Configs
{
    public class JsonConfigSerializer : IConfigSerializer
    {
        private readonly JsonSerializerSettings _settings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
        };

        public IGameConfigScheme Deserialize(string content, Type type)
        {
            return JsonConvert.DeserializeObject(content, type, _settings) as IGameConfigScheme;
        }

        public string Serialize(IGameConfigScheme configScheme)
        {
            return JsonConvert.SerializeObject(configScheme, _settings);
        }
    }
}