using System;
using System.Collections.Generic;
using CityBuilder.Grid;
using GameSystems.Common.ViewSystem.ViewsProvider;
using GameSystems.Implementation;
using GameSystems.Implementation.BattleSystem;
using GameSystems.Implementation.BuildingSystem;
using GameSystems.Implementation.BuildingSystem.Domain;
using GameSystems.Implementation.BuildingSystem.Features;
using GameSystems.Implementation.CheatsFeature;
using GameSystems.Implementation.GameInteractionFeature;
using GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine;
using GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States;
using GameSystems.Implementation.GameTime;
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
    public class GameSystemsInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // VIEW SYSTEM
            
            builder.Register<ViewsProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ViewWithModelProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<WindowsProvider>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            builder.Register<PlayerInputManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<PlayerInputSystem>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.RegisterInstance<DateModel>(new DateModel()).AsSelf();
            builder.Register<GameTimeSystem>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            builder.Register<PlayerResourcesModel>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<ResourcesManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            builder.Register<GridManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<CellGridFeature>(Lifetime.Singleton).AsImplementedInterfaces();
            
            // INTERACTION SYSTEM
            
            RegisterInteractionSystem(builder);

            // BUILDING SYSTEM
            
            builder.Register<BuildingFactory>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<BuildingsModel>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<BuildingManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<BuildingsViewFeature>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            builder.Register<MergeBuildingsFeature>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            // PRODUCTION SYSTEM
            
            builder.Register<ProductionModel>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<ResourcesProductionFeature>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ResourcesStorageFeature>(Lifetime.Singleton).AsImplementedInterfaces();
            
            // POPULATION SYSTEM

            builder.RegisterInstance<PopulationModel>(new PopulationModel()).AsSelf().AsImplementedInterfaces();
            builder.Register<PopulationFeature>(Lifetime.Singleton).AsImplementedInterfaces();
            
            // BATTLE SYSTEM
            
            builder.Register<BattleSystemModel>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<BattleManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<BattleUnitsProcessor>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<PlayerBuildingsUnitsController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<BattleFeature>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            // DEBUG
            
            builder.Register<CheatsFeature>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.RegisterComponentOnNewGameObject<DebugWindow>(Lifetime.Singleton, "DebugWindow");
        }

        private static void RegisterInteractionSystem(IContainerBuilder builder)
        {
            builder.Register<InteractionModel>(Lifetime.Singleton).AsSelf();
            builder.Register<CursorController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<Raycaster>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<DraggingContentController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<CellSelectionController>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            builder.Register<EmptyInteractionState>(Lifetime.Singleton).As<InteractionState>();
            builder.Register<CellSelectedInteractionState>(Lifetime.Singleton).As<InteractionState>();
            builder.Register<DraggingInteractionState>(Lifetime.Singleton).As<InteractionState>();
            builder.Register<PlayerInteractionStateMachine>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            builder.Register<GameInteractionFeature>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
    }
}