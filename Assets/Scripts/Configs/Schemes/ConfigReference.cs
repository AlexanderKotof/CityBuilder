using System;
using Newtonsoft.Json;

namespace Configs.Schemes
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ConfigReference<T> where T : IGameConfigPiece
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        
        [JsonIgnore]
        public T Value { get; internal set; }

        public ConfigReference() { }

        public ConfigReference(Guid id)
        {
            Id = id;
        }

        public ConfigReference(T value)
        {
            Value = value;
            Id = value.Id;
        }
        
        public static implicit operator ConfigReference<T>(T value) => new (value);
        
        public static implicit operator T(ConfigReference<T> reference) => reference.Value;
    }
}