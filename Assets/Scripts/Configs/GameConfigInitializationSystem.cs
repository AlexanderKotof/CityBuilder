using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Configs.Schemes
{
    public class GameConfigInitializationSystem
    {
        public GameConfigProvider GameConfigProvider { get; } = new();

        public GameConfigInitializationSystem()
        {
            
        }

        public async Task LoadConfigs(string path)
        {
            Debug.Log($"Begin load configs from {path}...");
            
            string[] files = Directory.GetFiles(path, "*.json");
            IEnumerable<Type> configTypes = ConfigTypesUtility.GetAllConfigTypes();
            
            Debug.Log($"Found {files.Length} config files.");

            foreach (Type configType in configTypes)
            {
                string fileName = configType.GetConfigFileName();
                foreach (var file in files)
                {
                    if (file.EndsWith(fileName, StringComparison.InvariantCultureIgnoreCase) == false)
                    {
                        continue;
                    }
                    
                    Debug.Log($"Trying to read {file}...");
                    
                    string fileContent = await File.ReadAllTextAsync(file);
                    IGameConfigScheme configScheme = JsonConvert.DeserializeObject(fileContent, configType) as IGameConfigScheme;
                    
                    Debug.Log($"Registering config {configScheme} by type {configType.Name}");
                    
                    GameConfigProvider.Register(configScheme, configType);
                }
            }
        }
    }
}