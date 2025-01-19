using System;
using System.Collections.Generic;
using UnityEngine;

namespace ViewSystem
{
    public class WindowsViewsRegistrator
    {
        public List<GameObject> WindowsPrefabs = new List<GameObject>();
        
        private Dictionary<string, IWindow> _windowsPrefabs = new Dictionary<string, IWindow>();

        public WindowsViewsRegistrator(List<GameObject> windowsPrefabs)
        {
            WindowsPrefabs = windowsPrefabs;

            foreach (var prefab in windowsPrefabs)
            {
                var windowComponent = prefab.GetComponent<IWindow>();
                if (windowComponent != null)
                {
                    _windowsPrefabs.Add(windowComponent.AssetId, windowComponent);
                    Debug.Log($"Registered window with id {windowComponent.AssetId} for {prefab.name}");
                }
            }
        }

        public WindowBase<TViewModel> GetWindow<TViewModel>()
            where TViewModel : IViewModel
        {
            var key = typeof(TViewModel).Name;
            if (_windowsPrefabs.TryGetValue(key, out IWindow window))
            {
                if (window is WindowBase<TViewModel> windowBase)
                {
                    return windowBase;
                }
                
                throw new InvalidCastException($"Cannot cast window {key} to type {typeof(WindowBase<TViewModel>).Name}!");
            }
            
            throw new KeyNotFoundException($"There is no window with name {key}");
        }
    }
}