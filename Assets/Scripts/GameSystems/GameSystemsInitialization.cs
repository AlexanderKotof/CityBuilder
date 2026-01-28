using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CityBuilder.Dependencies;
using UnityEngine;

namespace GameSystems
{
    [Obsolete]
    public class GameSystemsInitialization
    {
        private readonly IDependencyContainer _container;
    
        private static readonly Type[] DependencyContainerType = { typeof(IDependencyContainer) };
        private readonly object[] _constructorParameters;
        
        private readonly List<IGameSystem> _allGameSystems = new();
        private readonly List<IUpdateGamSystem> _updateGameSystems = new();

        public GameSystemsInitialization(IDependencyContainer container)
        {
            _container = container;
            _constructorParameters  = new object[] { _container };
        }

        public async Task Init()
        {
            var systems = CreateGameSystems(GameSystemsSets.CommonSystems, _container, _constructorParameters);
            
            await InitializeGameSystems(systems);
            
            //TODO: turn into states
            systems = CreateGameSystems(GameSystemsSets.GamePlayFeatures, _container, _constructorParameters);

            await InitializeGameSystems(systems);
            
            //TODO: Add systems dismounting
        }

        public Task Deinit()
        {
            return Task.WhenAll(_allGameSystems.Select<IGameSystem, Task>(system => system.Deinit()));
        }

        public void Update()
        {
            foreach (var gameSystem in _updateGameSystems)
            {
                gameSystem.Update();
            }
        }
            
        private async Task InitializeGameSystems(IReadOnlyCollection<IGameSystem> gameSystems)
        {
            foreach (var gameSystem in gameSystems)
            {
                Debug.Log($"Begin of initialization {gameSystem.GetType().Name}...");
                
                await gameSystem.Init();
                
                _allGameSystems.Add(gameSystem);
                
                if (gameSystem is IUpdateGamSystem updateGamSystem)
                {
                    _updateGameSystems.Add(updateGamSystem);
                }
                
                Debug.Log($"Successfully initialized {gameSystem.GetType().Name}!");
            }
        }
        
        private static IReadOnlyCollection<IGameSystem> CreateGameSystems(
            IReadOnlyCollection<Type> systemsToSet, 
            IDependencyContainer container,
            object[] constructorParameters)
        {
            Debug.Log("Begin initializing game systems");
            
            var gameSystems = new List<IGameSystem>();

            foreach (var systemType in systemsToSet)
            {
                object? system;
            
                ConstructorInfo constructorWithDependencies = systemType.GetConstructor(DependencyContainerType);

                if (constructorWithDependencies != null)
                { 
                    system = constructorWithDependencies.Invoke(constructorParameters);
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
                
                gameSystems.Add(gameSystem);

                RegisterPublicProperties(systemType, system, container);

                container.Register(systemType, gameSystem);
            }
            
            return gameSystems;
        }

        private static void RegisterPublicProperties(Type systemType, object system, IDependencyContainer container)
        {
            PropertyInfo[] properties = systemType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string))
                {
                    continue;
                }
                
                object? value = propertyInfo.GetValue(system);
                if (value != null)
                {
                    Debug.Log($"Registered dependency from {systemType.Name} - {propertyInfo.PropertyType.Name}.");
                    container.Register(propertyInfo.PropertyType, value);
                }
            }
        }
    }
}