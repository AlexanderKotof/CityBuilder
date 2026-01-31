using System;
using CityBuilder.Grid;
using Configs.Scriptable;
using Cysharp.Threading.Tasks;
using GameSystems.Common.ViewSystem;
using GameSystems.Common.ViewSystem.ViewsProvider;
using GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States;
using UnityEditor.AddressableAssets.GUI;
using UnityEngine;
using VContainer.Unity;
using ViewSystem;

namespace GameSystems.Implementation.GameInteractionFeature
{
    public class CursorController : IInitializable, IDisposable
    {
        private readonly IViewsProvider _provider;
        private readonly CommonGameSettingsSo _settings;
        private CursorComponent _singleCursor;
        
        private CellModel[] _lightenedCells;
        
        private readonly ViewsCollectionController<CursorComponent> _cursorsController;

        public CursorController(IViewsProvider provider, CommonGameSettingsSo settings)
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
        
        private async UniTask LoadCursorView()
        {
            _singleCursor = await _provider.ProvideViewAsync<CursorComponent>(_settings.SelectorAssetReferenceKey);
            SetActive(false);
        }

        public void SetPosition(Vector3 position, Vector2Int selectionSize)
        {
            if (_singleCursor == null)
            {
                return;
            }

            _singleCursor.SetCursor(position, new Vector3(selectionSize.x, 1, selectionSize.y));
        }
        
        public void SetPositions(CellModel[] lightenCells, CursorStateEnum cursorState)
        {
            if (_lightenedCells != null)
            {
                _cursorsController.Dispose();
            }
            
            _lightenedCells = lightenCells;
            
            foreach (var cell in _lightenedCells)
            {
                AddView(cell).Forget();
            }
            return;

            async UniTaskVoid AddView(CellModel cellModel)
            {
                var view = await _cursorsController.AddView(cellModel);
                view.Setup(cellModel, cursorState);
            }
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
            _lightenedCells = null;
        }
    }
}