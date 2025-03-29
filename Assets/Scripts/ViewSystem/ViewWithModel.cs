using System;
using System.Collections.Generic;
using CityBuilder.Reactive;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using ViewSystem.Implementation;

namespace ViewSystem
{
    public interface IView
    {

    }

    public abstract class ViewBase : MonoBehaviour, IView
    {
        
    }
    
    public interface IWindow
    {
        string AssetId { get; }
    }

    public abstract class WindowBase<TViewModel> : ViewWithModel<TViewModel>, IWindow
        where TViewModel : IViewModel
    {
        public string AssetId => typeof(TViewModel).Name;
    }
    
    public abstract class ViewWithModel<TModel> : ViewBase
        where TModel : IViewModel
    {
        public TModel Model { get; private set; }
        
        private readonly List<Action> _deinitActions = new List<Action>(); 

        public virtual void Initialize(TModel model)
        {
            Model = model;
        }
        
        public virtual void Deinit()
        {
            foreach (var action in _deinitActions)
            {
                action.Invoke();
            }
            _deinitActions.Clear();
        }

        protected void Subscribe<T>(ReactiveProperty<T> property, Action<T> handler, bool invokeOnSubscribe = true)
        {
            property.AddListener(handler);
            _deinitActions.Add(() => property.RemoveListener(handler));

            if (invokeOnSubscribe)
            {
                handler.Invoke(property.Value);
            }
        }
    }
}