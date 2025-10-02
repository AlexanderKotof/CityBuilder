using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Configs.Extensions;
using Configs.Schemes;
using Configs.Utilities;
using GameSystems;
using UnityEngine;

namespace Configs
{
    public class GameConfigInitializationSystem : IGameSystem
    {
        public GameConfigProvider GameConfigProvider { get; } = new();
        
        private readonly IConfigSerializer _configSerializer = new JsonConfigSerializer();
        private readonly GameConfigsProcessor _configsProcessor = new();
        
        public Task Init()
        {
            string path = PathUtility.ConfigsPath;
            return LoadConfigs(path);
        }

        public Task Deinit()
        {
            return Task.CompletedTask;
        }
        
        private async Task LoadConfigs(string path)
        {
            _configsProcessor.Clear();
            
            var list = new List<(IGameConfigScheme, Type)>();
            
            Debug.Log($"Begin load configs from {path}...");
            
            string[] files = Directory.GetFiles(path, "*.json");
            Type[] configTypes = ConfigTypeUtility.GetAllConfigTypes().ToArray();
            
            Debug.Log($"Found {files.Length} config files.");

            foreach (Type configType in configTypes)
            {
                string fileName = configType.GetConfigFileName();
                string filePath = files.FirstOrDefault(file =>
                    file.EndsWith(fileName, StringComparison.InvariantCultureIgnoreCase));

                if (string.IsNullOrEmpty(filePath))
                {
                    Debug.LogError($"No file found for config {configType.Name}...");
                    continue;
                }
                    
                string fileContent = await File.ReadAllTextAsync(filePath);
                IGameConfigScheme configScheme = _configSerializer.Deserialize(fileContent, configType);
                
                list.Add((configScheme, configType));
                _configsProcessor.CollectReferences(configScheme);
            }
            
            foreach (var (configScheme, configType) in list)
            {
                _configsProcessor.ResolveReferences(configScheme);
                GameConfigProvider.Register(configScheme, configType);
                
                Debug.Log($"Registered config {configScheme} by type {configType.Name}");
            }
        }
    }
}