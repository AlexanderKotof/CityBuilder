using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Configs.Schemes;
using GameSystems;
using UnityEngine;

namespace Configs
{
    public class GameConfigInitializationSystem : IGameSystem
    {
        public GameConfigProvider GameConfigProvider { get; } = new();
        
        private readonly IConfigSerializer _configSerializer = new JsonConfigSerializer();
        private readonly IConfigLoader _configLoader = new LocalConfigLoader();
        private readonly GameConfigsProcessor _configsProcessor = new();
        
        public Task Init()
        {
            return LoadConfigs();
        }

        public Task Deinit()
        {
            return Task.CompletedTask;
        }
        
        private async Task LoadConfigs()
        {
            _configsProcessor.Clear();
            
            var configsRaw = await _configLoader.LoadConfigs();
            var configs = new List<IGameConfigScheme>(configsRaw.Count);

            foreach ((Type type, string content) configRaw in configsRaw)
            {
                IGameConfigScheme configScheme = _configSerializer.Deserialize(configRaw.content, configRaw.type);
                
                configs.Add(configScheme);
                
                _configsProcessor.CollectReferences(configScheme);
            }
            
            foreach (var configScheme in configs)
            {
                _configsProcessor.ResolveReferences(configScheme);
                GameConfigProvider.Register(configScheme, configScheme.GetType());
                
                Debug.Log($"Registered config {configScheme} by type {configScheme.GetType().Name}");
            }
        }
    }
}