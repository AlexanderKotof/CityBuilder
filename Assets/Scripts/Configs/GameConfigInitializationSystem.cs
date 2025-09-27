using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Configs.Extensions;
using Configs.Schemes;
using Configs.Utilities;
using Newtonsoft.Json;
using UnityEngine;

namespace Configs
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
                
                Debug.Log($"Trying to read {filePath}...");
                    
                string fileContent = await File.ReadAllTextAsync(filePath);
                IGameConfigScheme configScheme = JsonConvert.DeserializeObject(fileContent, configType) as IGameConfigScheme;
                    
                Debug.Log($"Registering config {configScheme} by type {configType.Name}");
                    
                GameConfigProvider.Register(configScheme, configType);
            }
        }
    }
}