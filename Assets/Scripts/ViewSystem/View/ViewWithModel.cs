using System;
using System.Collections.Generic;
using CityBuilder.Dependencies;
using CityBuilder.Reactive;
using JetBrains.Annotations;

namespace ViewSystem
{
    public abstract class ViewWithModel<TModel> : ViewBase
        where TModel : IViewModel
    {
        public TModel Model { get; private set; }
        public IDependencyContainer Container { get; private set; }
        
        private readonly List<Action> _deinitActions = new(); 

        public virtual void Initialize(TModel model, IDependencyContainer dependencies)
        {
            Model = model;
            Container = dependencies;
        }

        public virtual void Deinit()
        {
            foreach (var action in _deinitActions)
            {
                action.Invoke();
            }
            _deinitActions.Clear();
        }
        
        protected void InitView<TViewModel>([CanBeNull] ViewWithModel<TViewModel> view, TViewModel model)
            where TViewModel : IViewModel
        {
            if (view == null)
            {
                return;
            }
            
            view.Initialize(model, Container);
            _deinitActions.Add(view.Deinit);
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