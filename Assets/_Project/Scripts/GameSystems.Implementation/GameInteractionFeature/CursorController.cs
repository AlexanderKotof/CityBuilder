using System;
using System.Collections.Generic;
using CityBuilder.Configs.Scriptable;
using CityBuilder.GameSystems.Common.ViewSystem;
using CityBuilder.GameSystems.Common.ViewSystem.ViewsProvider;
using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using CityBuilder.GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.GameInteractionFeature
{
    public class CursorController : IInitializable, IDisposable, ITickable
    {
        private readonly IViewsProvider _provider;
        private readonly CommonGameSettingsSo _settings;
        private CursorComponent _singleCursor;
        
        private readonly ViewsCollectionController<CursorComponent> _cursorsController;

        public CursorController(
            IViewsProvider provider, 
            CommonGameSettingsSo settings)
        {
            _provider = provider;
            _settings = settings;
            _cursorsController = new ViewsCollectionController<CursorComponent>(_provider, defaultAssetKey: _settings.SelectorAssetReferenceKey);
        }

        public void Initialize()
        {
            LoadCursorView().Forget();
        }

        public void Dispose()
        {
            _provider.ReturnView(_singleCursor);
            _cursorsController.Dispose();
        }
    
        public void Tick()
        {
            
        }
        
        private async UniTask LoadCursorView()
        {
            _singleCursor = await _provider.ProvideViewAsync<CursorComponent>(_settings.SelectorAssetReferenceKey);
            SetActive(false);
        }

        public void SetSelection(Vector3 position, Vector2Int selectionSize)
        {
            if (_singleCursor == null)
            {
                return;
            }

            _singleCursor.SetCursor(position, new Vector3(selectionSize.x, 1, selectionSize.y));
        }
        
        public void SetPositions(IEnumerable<CellModel> lightenCells, CursorStateEnum cursorState)
        {
            foreach (var cell in lightenCells)
            {
                GetOrAddView(cell, cursorState).Forget();
            }
        }
        
        public void SetPosition(CellModel lightenCell, CursorStateEnum cursorState)
        {
            GetOrAddView(lightenCell, cursorState).Forget();
        }
        
        async UniTaskVoid GetOrAddView(CellModel cellModel, CursorStateEnum cursorState)
        {
            if (_cursorsController.TryGetView(cellModel, out var view) == false)
            {
                view = await _cursorsController.AddView(cellModel);
            }
            view.Setup(cellModel, cursorState);
        }

        public void SetActive(bool active)
        {
            if (_singleCursor == null)
            {
                return;
            }
            
            _singleCursor.gameObject.SetActive(active);
        }

        public void Clear()
        {
            _cursorsController.Dispose();
        }
    }
}