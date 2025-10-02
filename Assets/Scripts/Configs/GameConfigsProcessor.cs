using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Configs.Schemes;
using UnityEngine;

namespace Configs
{
    public class GameConfigsProcessor
    {
        private readonly Dictionary<Guid, IGameConfigPiece> _configById = new();
        
        public void CollectReferences(object obj)
        {
            if (obj == null)
                return;
            
            var type = obj.GetType();
            if (IsComplexType(type) == false)
                return;

            // Если это ConfigBase — добавляем в словарь
            if (obj is ConfigBase config)
            {
                if (_configById.TryAdd(config.Id, config) == false)
                {
                    throw new InvalidOperationException($"Duplicate config ID: {config.Id} ({config.GetType().Name})");
                }
            }

            // Коллекции
            if (obj is IEnumerable enumerable && !(obj is string))
            {
                foreach (var item in enumerable)
                {
                    CollectReferences(item);
                }
                return;
            }

            // Обычные объекты
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead) continue;
                var value = prop.GetValue(obj);
                CollectReferences(value);
            }

            // foreach (var prop in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            // {
            //     var value = prop.GetValue(obj);
            //     CollectReferences(value);
            // }
        }
        
        public void ResolveReferences(object targetObject)
        {
            if (targetObject == null) return;
            
            Type type = targetObject.GetType();
            
            // Примитивы и строки — не обрабатываем
            if (IsComplexType(type) == false)
                return;
            
            // Обработка ConfigReference<T>
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ConfigReference<>))
            {
                var idProp = type.GetProperty("Id");
                var valueProp = type.GetProperty("Value");

                var id = (Guid)(idProp?.GetValue(targetObject) ?? Guid.Empty);
                if (id != Guid.Empty && _configById.TryGetValue(id, out var target))
                {
                    var targetType = type.GetGenericArguments()[0];
                    if (targetType.IsAssignableFrom(target.GetType()))
                    {
                        valueProp?.SetValue(targetObject, target);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"Type mismatch: ConfigReference<{targetType.Name}> cannot reference {target.GetType().Name}");
                    }
                }
                return; // ConfigReference — лист, внутрь не лезем
            }
            
            // Коллекции
            if (targetObject is IEnumerable enumerable && !(targetObject is string))
            {
                foreach (var item in enumerable)
                {
                    ResolveReferences(item);
                }
                return;
            }

            // Обычные объекты — обходим свойства
            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanRead) continue;
                var value = prop.GetValue(targetObject);
                ResolveReferences(value);
            }
            
            // foreach (var prop in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            // {
            //     var value = prop.GetValue(targetObject);
            //     ResolveReferences(value);
            // }
        }

        private static bool IsComplexType(Type type)
        {
            return !type.IsPrimitive &&
                   type != typeof(string) &&
                   type != typeof(Guid) &&
                   type != typeof(DateTime) &&
                   !type.IsEnum;
        }
    }
}