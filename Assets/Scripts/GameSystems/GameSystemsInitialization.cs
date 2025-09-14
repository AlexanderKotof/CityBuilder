using System;
using System.Collections.Generic;
using System.Reflection;
using CityBuilder.Dependencies;
using PeopleFeature;
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
        };
    }
    
    public class GameSystemsInitialization : IGameSystem
    {
        private readonly IDependencyContainer _container;
    
        private readonly Type[] _dependencyContainerType = new [] { typeof(IDependencyContainer) };
        private readonly object[] _constructorParameters;
        
        public readonly List<IGameSystem> _gameSystems = new();
        public readonly List<IUpdateGamSystem> _updateGameSystems = new();

        public GameSystemsInitialization(IDependencyContainer container)
        {
            _container = container;
            _constructorParameters  = new [] { _container };
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
                
                FieldInfo[] fields = systemType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    var value = field.GetValue(system);
                    if (value != null)
                    {
                        Debug.Log($"Registered dependency from {systemType.Name} - {field.FieldType.Name}.");
                        _container.Register(field.FieldType, value);
                    }
                }

                _container.Register(systemType, gameSystem);
            }
        }
    
        private void InitializeGameSystems()
        {
            foreach (var gameSystem in _gameSystems)
            {
                gameSystem.Init();
            }
        }

    }
}