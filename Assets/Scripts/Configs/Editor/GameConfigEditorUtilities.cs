using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Configs.Schemes;
using Unity.Plastic.Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Configs.Editor
{
    public static class GameConfigEditorUtilities
    {
        private static readonly string Path = PathUtility.ConfigsPath;
        
        [MenuItem("Tools/Configs/Create...")]
        public static async void CreateConfigsWithExistingSchemesIfNotExist()
        {
            Debug.Log($"Begin creating configs from {Path}!");
            
            var files = Directory.GetFiles(Path, "*.json");
            var configTypes = ConfigTypesUtility.GetAllConfigTypes().ToArray();
            
            Debug.Log($"Found {configTypes.Length} config types and {files.Length} files.");
            foreach (var configType in configTypes)
            {
                Debug.Log($"Config: {configType}");
            }
            
            foreach (var file in files)
            {
                Debug.Log($"File: {file}");
            }

            foreach (Type configType in configTypes)
            {
                string fileName = configType.GetConfigFileName();
                
                if (IsFileExist(fileName, files))
                {
                    Debug.Log($"Config: {fileName} already exists.");
                    
                    await TryUpdateFile(fileName, configType);
                    
                    continue;
                }
                
                var config = (IGameConfigScheme)Activator.CreateInstance(configType);
                var json = JsonConvert.SerializeObject(config);
                
                await File.WriteAllTextAsync(Path + fileName, json);
                
                Debug.Log($"Created config {fileName}");
            }
        }
        
        private static bool IsFileExist(string fileName, string[] files)
        {
            return files.Any(filePath => filePath.EndsWith(fileName, StringComparison.InvariantCultureIgnoreCase));
        }

        private static async Task TryUpdateFile(string fileName, Type configType)
        {
            string fileContent = await File.ReadAllTextAsync(Path + fileName);
            var config = JsonConvert.DeserializeObject(fileContent, configType) as IGameConfigScheme;
            var json = JsonConvert.SerializeObject(config);
                
            await File.WriteAllTextAsync(Path + fileName, json);
            
            Debug.Log($"Successfully updated {fileName}...");
        }
    }
}