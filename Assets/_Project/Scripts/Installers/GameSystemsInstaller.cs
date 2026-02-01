using CityBuilder.GameSystems.Common.ViewSystem.ViewsProvider;
using CityBuilder.GameSystems.Common.WindowSystem;
using CityBuilder.GameSystems.Implementation;
using CityBuilder.GameSystems.Implementation.BattleSystem;
using CityBuilder.GameSystems.Implementation.BattleSystem.Domain;
using CityBuilder.GameSystems.Implementation.BattleSystem.Features;
using CityBuilder.GameSystems.Implementation.BattleSystem.Processing;
using CityBuilder.GameSystems.Implementation.BuildingSystem;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Features;
using CityBuilder.GameSystems.Implementation.CellGridFeature;
using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using CityBuilder.GameSystems.Implementation.CheatsFeature;
using CityBuilder.GameSystems.Implementation.GameInteractionFeature;
using CityBuilder.GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine;
using CityBuilder.GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States;
using CityBuilder.GameSystems.Implementation.GameTime;
using CityBuilder.GameSystems.Implementation.HudWindow;
using CityBuilder.GameSystems.Implementation.PopulationFeature;
using CityBuilder.GameSystems.Implementation.ResourcesFeature.Core;
using CityBuilder.GameSystems.Implementation.ResourcesFeature.Core.Domain;
using CityBuilder.GameSystems.Implementation.ResourcesFeature.ProducingFeature;
using CityBuilder.GameSystems.Implementation.ResourcesFeature.ResourcesStorageFeature;
using CityBuilder.PlayerInput;
using VContainer;
using VContainer.Unity;

namespace CityBuilder.Installers
{
    public class GameSystemsInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // COMMON SYSTEMS
            
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
            
            builder.Register<BattleUnitsFactory>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<BattleSystemModel>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<BattleManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<BattleUnitsProcessor>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            builder.Register<BattleUnitsViewFeature>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            builder.Register<PlayerBuildingsUnitsFeature>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            // WINDOWS
            
            builder.Register<HudWindowFeature>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
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
            builder.Register<PlayerActionsService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            builder.Register<GameInteractionFeature>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
    }
}