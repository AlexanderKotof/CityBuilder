using System;
using System.Collections.Generic;
using CityBuilder.BuildingSystem;
using CityBuilder.Grid;
using GameSystems.Implementation;
using GameSystems.Implementation.BattleSystem;
using GameSystems.Implementation.BuildingsFeature;
using GameSystems.Implementation.CheatsFeature;
using GameSystems.Implementation.GameInteractionFeature;
using GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine;
using GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States;
using GameSystems.Implementation.GameTimeSystem;
using GameSystems.Implementation.PopulationFeature;
using GameSystems.Implementation.ProducingFeature;
using GameSystems.Implementation.ResourcesStorageFeature;
using PlayerInput;
using ResourcesSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using ViewSystem;

namespace Installers
{
    public class GameSystemsInstaller : LifetimeScope, IInstaller
    {
        public static readonly HashSet<Type> GamePlayFeatures = new()
        {
            typeof(BuildingsViewFeature),
            typeof(ResourcesProductionFeature),
            typeof(PopulationFeature),
            typeof(ResourcesStorageFeature),
            typeof(BattleFeature),
            typeof(CheatsFeature),
        };
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ViewsProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ViewWithModelProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            //builder.Register<ViewsSystem>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<WindowsProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            //builder.Register<WindowSystem>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<PlayerInputManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayerInputSystem>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.RegisterInstance<DateModel>(new DateModel(100,1,1)).AsSelf();
            builder.Register<GameTimeSystem>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            builder.Register<PlayerResourcesModel>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<ResourcesManager>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<GridManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<CellGridFeature>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<CursorController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<Raycaster>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<DraggingContentController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<InteractionModel>(Lifetime.Singleton).AsSelf();
            builder.Register<CellSelectionController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            builder.Register<EmptyInteractionState>(Lifetime.Singleton).As<InteractionState>();
            builder.Register<CellSelectedInteractionState>(Lifetime.Singleton).As<InteractionState>();
            builder.Register<DraggingInteractionState>(Lifetime.Singleton).As<InteractionState>();
            builder.Register<PlayerInteractionStateMachine>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            builder.RegisterEntryPoint<GameInteractionFeature>().AsImplementedInterfaces();

            builder.Register<BuildingFactory>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<BuildingsModel>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<BuildingManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<BuildingsViewFeature>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            
            // Debug.Log("Begin common systems registration");
            // foreach (var system in CommonSystems)
            // {
            //     builder.Register(system, Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            // }
            //
            // Debug.Log("Begin GamePlayFeatures registration");
            // foreach (var system in GamePlayFeatures)
            // {
            //     builder.Register(system, Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            // }
        }
        
        

        public void Install(IContainerBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}