using Configs.Scriptable;
using UnityEngine;
using VContainer.Unity;
using ViewSystem;

namespace GameSystems.Implementation.GameInteractionFeature
{
    public class CursorController : IInitializable
    {
        private readonly IViewsProvider _provider;
        private readonly CommonGameSettingsSO _settings;
        private Transform _cursor;

        public CursorController(IViewsProvider provider, CommonGameSettingsSO settings)
        {
            _provider = provider;
            _settings = settings;
        }

        public async void Initialize()
        {
            _cursor = await _provider.ProvideViewAsync<Transform>(_settings.SelectorAssetReferenceKey);
            SetActive(false);
        }

        public void SetPosition(Vector3 position, Vector2Int selectionSize)
        {
            if (_cursor == null)
            {
                return;
            }
            
            _cursor.position = position;
            _cursor.localScale = new Vector3(selectionSize.x, 1, selectionSize.y);
        }

        public void SetActive(bool active)
        {
            if (_cursor == null)
            {
                return;
            }
            
            _cursor.gameObject.SetActive(active);
        }
    }
}