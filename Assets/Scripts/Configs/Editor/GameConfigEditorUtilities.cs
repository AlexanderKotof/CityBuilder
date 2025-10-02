using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Configs.Extensions;
using Configs.Schemes;
using Configs.Utilities;
using UnityEditor;
using UnityEngine;

namespace Configs.Editor
{
    public static class GameConfigEditorUtilities
    {
        private static readonly string Path = PathUtility.ConfigsPath;

        private static readonly IConfigSerializer Serializer = new JsonConfigSerializer();
        private static readonly GameConfigsProcessor ConfigsProcessor = new GameConfigsProcessor();
        
        [MenuItem("Tools/Configs/Rebuild...")]
        public static async void CreateConfigsWithExistingSchemesIfNotExist()
        {
            ConfigsProcessor.Clear();
            
            Debug.Log($"Begin creating configs from {Path}!");
            
            var files = Directory.GetFiles(Path, "*.json");
            var configTypes = ConfigTypeUtility.GetAllConfigTypes().ToArray();
            
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
                var json = Serializer.Serialize(config);
                
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
            var config = Serializer.Deserialize(fileContent, configType);
            
            ValidateConfig(config);

            var json = Serializer.Serialize(config);
                
            await File.WriteAllTextAsync(Path + fileName, json);
            
            Debug.Log($"Successfully updated {fileName}...");
        }

        private static void ValidateConfig(IGameConfigScheme config)
        {
            try
            {
                ConfigsProcessor.CollectReferences(config);
                ConfigsProcessor.ResolveReferences(config);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }
    }
}