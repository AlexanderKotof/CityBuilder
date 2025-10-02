using System;
using System.Collections.Generic;
using CityBuilder.BuildingSystem;
using Configs.Schemes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using HouseHoldsIncreaseBuildingFunction = CityBuilder.BuildingSystem.HouseHoldsIncreaseBuildingFunction;

namespace Configs.Converter
{
    // public class Vector2IntConverter : JsonConverter<Vector2Int>
    // {
    //     public override void WriteJson(JsonWriter writer, Vector2Int value, JsonSerializer serializer)
    //     {
    //         JObject jo = JObject.FromObject(value, serializer);
    //         jo.WriteTo(writer);
    //     }
    //
    //     public override Vector2Int ReadJson(JsonReader reader, Type objectType, Vector2Int existingValue, bool hasExistingValue,
    //         JsonSerializer serializer)
    //     {
    //         // Загружаем JSON как JObject, чтобы прочитать поле "type"
    //         JObject jo = JObject.Load(reader);
    //
    //         // Получаем значение дискриминатора
    //         string type = jo["type"]?.Value<string>() ?? jo["Type"]?.Value<string>();
    //
    //         if (string.IsNullOrEmpty(type))
    //             throw new JsonSerializationException("Missing 'type' field in JSON.");
    //
    //         // Определяем тип на основе значения
    //         T target = _configTypesRegistry.GetConfigPiece(type.ToLowerInvariant()) as T;
    //
    //         // Заполняем свойства объекта из JSON
    //         serializer.Populate(jo.CreateReader(), target);
    //         return target;
    //     }
    // }
    
    
    public class TypesConverter<T> : JsonConverter<T>
        where T : class, IGameConfigPiece
    {
        private readonly ConfigTypesRegistry _configTypesRegistry = new();
        
        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            // Просто сериализуем объект как есть (включая поле "type")
            JObject jo = JObject.FromObject(value, serializer);
            jo.WriteTo(writer);
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Загружаем JSON как JObject, чтобы прочитать поле "type"
            JObject jo = JObject.Load(reader);

            // Получаем значение дискриминатора
            string type = jo["type"]?.Value<string>() ?? jo["Type"]?.Value<string>();

            if (string.IsNullOrEmpty(type))
                throw new JsonSerializationException("Missing 'type' field in JSON.");

            // Определяем тип на основе значения
            T target = _configTypesRegistry.GetConfigPiece(type.ToLowerInvariant()) as T;

            // Заполняем свойства объекта из JSON
            serializer.Populate(jo.CreateReader(), target);
            return target;
        }
    }

    public class ConfigTypesRegistry
    {
        private readonly Dictionary<string, Func<IGameConfigPiece>> _map = new()
        {
            ["main-building-func"] = () => new MainBuildingFunction(),
            ["production-building-func"] = () => new ResourceProductionBuildingFunction(),
            ["house-building-func"] = () => new HouseHoldsIncreaseBuildingFunction(),
            ["mercenary-building-func"] = () => new MercenaryBuildingFunction(),
            ["storage-building-func"] = () => new ResourceStorageBuildingFunction(),
        };

        public IGameConfigPiece GetConfigPiece(string shortType)
        {
            if (_map.TryGetValue(shortType, out Func<IGameConfigPiece> func))
            {
                return func();
            }
            
            throw new JsonSerializationException($"Unknown function type: {shortType}");
        }
    }
}