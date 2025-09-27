using System;
using System.Collections.Generic;
using System.Reflection;
using CityBuilder.Dependencies;
using GameSystems.Implementation.CheatsFeature;
using GameSystems.Implementation.PopulationFeature;
using ResourcesSystem;
using UnityEngine;

namespace GameSystems
{
    public static class GameSystemsSet
    {
        public static readonly HashSet<Type> GameSystemTypes = new()
        {
            typeof(GameTimeSystem.GameTimeSystem),
            typeof(ProducingFeature.ResourcesProductionFeature),
            typeof(PopulationFeature),
            typeof(ResourcesStorageFeature),
            typeof(CheatsFeature),
        };
    }
    
    public class GameSystemsInitialization : IGameSystem
    {
        private readonly IDependencyContainer _container;
    
        private readonly Type[] _dependencyContainerType = new [] { typeof(IDependencyContainer) };
        private readonly object[] _constructorParameters;
        
        private readonly List<IGameSystem> _gameSystems = new();
        private readonly List<IUpdateGamSystem> _updateGameSystems = new();

        public GameSystemsInitialization(IDependencyContainer container)
        {
            _container = container;
            _constructorParameters  = new object[] { _container };
        }

        public void Init()
        {
            CreateGameSystems();

            InitializeGameSystems();
        }

        public void Deinit()
        {
            foreach (var gameSystem in _gameSystems)
            {
                gameSystem.Deinit();
            }
        }

        public void Update()
        {
            foreach (var gameSystem in _updateGameSystems)
            {
                gameSystem.Update();
            }
        }

            
        private void InitializeGameSystems()
        {
            foreach (var gameSystem in _gameSystems)
            {
                Debug.Log($"Begin of initialization {gameSystem.GetType().Name}...");
                gameSystem.Init();
            }
        }
        
        private void CreateGameSystems()
        {
            Debug.Log("Begin initializing Game Systems");

            foreach (var systemType in GameSystemsSet.GameSystemTypes)
            {
                object? system;
            
                ConstructorInfo constructorWithDependencies = systemType.GetConstructor(_dependencyContainerType);

                if (constructorWithDependencies != null)
                { 
                    system = constructorWithDependencies.Invoke(_constructorParameters);
                    Debug.Log($"System of type {systemType.Name} has constructor with dependencies");
                }
                else
                {
                    ConstructorInfo defaultConstructor = systemType.GetConstructor(Type.EmptyTypes);
                    system = defaultConstructor?.Invoke(null);
                    Debug.Log($"System of type {systemType.Name} has constructor without dependencies");
                }
            
                if (system == null)
                {
                    Debug.LogError($"System of type {systemType.Name} does not have a default constructors.");
                    continue;
                }
            
                if (system is not IGameSystem gameSystem)
                {
                    Debug.LogError($"System of type {systemType.Name} does not implement IGameSystem.");
                    continue;
                }

                Debug.Log($"Successfully created game system {systemType.Name}.");
                _gameSystems.Add(gameSystem);
                
                if (system is IUpdateGamSystem updateGamSystem)
                {
                    _updateGameSystems.Add(updateGamSystem);
                }
                
                RegisterPublicProperties(systemType, system);

                _container.Register(systemType, gameSystem);
            }
        }

        private void RegisterPublicProperties(Type systemType, object system)
        {
            PropertyInfo[] properties = systemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string))
                {
                    continue;
                }
                    
                Debug.Log($"Try register dependency from {systemType.Name} - {propertyInfo.PropertyType.Name}.");
                    
                object? value = propertyInfo.GetValue(system);
                if (value != null)
                {
                    Debug.Log($"Registered dependency from {systemType.Name} - {propertyInfo.PropertyType.Name}.");
                    _container.Register(propertyInfo.PropertyType, value);
                }
            }
        }
    }
}