using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Configs.Utilities;
using UnityEngine;

namespace Configs
{
    public class LocalConfigLoader : IConfigLoader
    {
        private readonly string _path = PathUtility.ConfigsPath;
        
        public async Task<IReadOnlyCollection<(Type, string)>> LoadConfigs()
        {
            Debug.Log($"Begin load configs from {_path}...");
            
            var list = new List<(Type, string)>();
            Type[] configTypes = ConfigTypeUtility.GetAllConfigTypes().ToArray();
            
            foreach (Type configType in configTypes)
            {
                string fileName = configType.GetConfigFileName();
                string filePath = _path + fileName;
                
                if (File.Exists(filePath) == false)
                {
                    Debug.LogError($"No file found for config {configType.Name}...");
                    continue;
                }
                    
                string fileContent = await File.ReadAllTextAsync(filePath);
                list.Add((configType, fileContent));
            }
            
            return list;
        }
    }
}