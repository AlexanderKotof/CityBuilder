using System;
using System.Threading;
using System.Threading.Tasks;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using CityBuilder.Grid;
using Configs;
using Configs.Extensions;
using Configs.Schemes;
using Configs.Scriptable;
using Cysharp.Threading.Tasks;
using GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine;
using GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States;
using UnityEngine;
using VContainer.Unity;
using ViewSystem;

namespace GameSystems.Implementation.GameInteractionFeature
{
    public class GameInteractionFeature : IAsyncStartable, IDisposable
    {
        private PlayerInteractionStateMachine? _playerInteractionStateMachine;
    
        private readonly IViewsProvider _viewsProvider;
        private readonly CommonGameSettingsSO _settings;
        private Transform _cursor;
        private CellSelectionController _cellSelectionController;

        public GameInteractionFeature(GameConfigProvider configProvider, Camera camera, GridManager gridManager, IViewsProvider viewsProvider)
        {
        }
        
        public async UniTask StartAsync(CancellationToken cancellation = new CancellationToken())
        {
            Debug.LogWarning("Game Interaction Feature is now running.");
            
            //TODO: ???
            //Container.Register(CursorController);

            // var buildingManager = Container.Resolve<BuildingManager>();
            // _cellSelectionController =
            //     new CellSelectionController(CursorController, InteractionModel, buildingManager);
            // _cellSelectionController.Init();
            //
            // _playerInteractionStateMachine = new PlayerInteractionStateMachine(
            //     new EmptyInteractionState(Container),
            //     new CellSelectedInteractionState(Container),
            //     new DraggingInteractionState(Container)
            // );
            //
            // _playerInteractionStateMachine.Start(typeof(EmptyInteractionState));
        }

        public void Dispose()
        {
        }
    }
}