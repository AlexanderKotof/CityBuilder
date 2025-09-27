using System;
using System.Collections.Generic;
using UnityEngine;

namespace ViewSystem
{
    public class WindowsViewsRegistrator
    {
        public List<GameObject> WindowsPrefabs = new List<GameObject>();
        
        private readonly Dictionary<Type, IWindow> _windowsPrefabsByViewModel = new ();

        public WindowsViewsRegistrator(List<GameObject> windowsPrefabs)
        {
            WindowsPrefabs = windowsPrefabs;

            foreach (var prefab in windowsPrefabs)
            {
                var windowComponent = prefab.GetComponent<IWindow>();
                if (windowComponent != null)
                {
                    _windowsPrefabsByViewModel.Add(windowComponent.ViewModelType, windowComponent);
                    Debug.Log($"Registered window with id {windowComponent.ViewModelType} for {prefab.name}");
                }
            }
        }

        public WindowViewBase<TViewModel> GetWindow<TViewModel>()
            where TViewModel : IViewModel
        {
            var key = typeof(TViewModel);
            if (_windowsPrefabsByViewModel.TryGetValue(key, out IWindow window))
            {
                if (window is WindowViewBase<TViewModel> windowBase)
                {
                    return windowBase;
                }
                
                throw new InvalidCastException($"Cannot cast window {key} to type {typeof(WindowViewBase<TViewModel>).Name}!");
            }
            
            throw new KeyNotFoundException($"There is no window with name {key}");
        }
    }
}