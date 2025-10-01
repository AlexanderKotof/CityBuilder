using System;
using Configs.Schemes;
using Newtonsoft.Json;

namespace Configs
{
    public class JsonConfigSerializer : IConfigSerializer
    {
        private readonly JsonSerializerSettings _settings;

        public JsonConfigSerializer()
        {
            _settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto // или TypeNameHandling.Objects
            };
        }
        
        public IGameConfigScheme Deserialize(string content, Type type)
        {
            return JsonConvert.DeserializeObject(content, type, _settings) as IGameConfigScheme;
        }

        public string Serialize(IGameConfigScheme configScheme)
        {
            return JsonConvert.SerializeObject(configScheme, Formatting.Indented, _settings);
        }
    }
}