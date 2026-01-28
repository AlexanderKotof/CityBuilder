using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace StateMachine
{
    public abstract class StateMachineBase<TState> : IStateMachine<TState>, IDisposable
        where TState : IState
    {
        public TState CurrentState { get; private set; }

        private readonly Dictionary<Type, TState> _statesMap = new Dictionary<Type, TState>();

        protected StateMachineBase(IEnumerable<TState> states)
        {
            foreach (var state in states)
            {
                _statesMap[state.GetType()] = state;
            }
        }
        
        protected StateMachineBase(params TState[] states)
        {
            foreach (var state in states)
            {
                _statesMap[state.GetType()] = state;
            }
        }

        public void Start(Type startState)
        {
            foreach (var state in _statesMap.Values)
            {
                state.ChangeStateRequested += ChangeState;
            }

            ChangeState(startState);

            OnStarted();
        }

        protected virtual void OnStarted() { }

        protected virtual void OnStoped() { }

        public void Stop()
        {
            foreach (var state in _statesMap.Values)
            {
                state.ChangeStateRequested -= ChangeState;
            }

            if (CurrentState != null)
            {
                CurrentState.ExitState();
            }

            OnStoped();

            CurrentState = default;
        }

        private void ChangeState(Type state)
        {
            if (CurrentState != null)
            {
                CurrentState.ExitState();
            }

            if (_statesMap.TryGetValue(state, out var newState))
            {
                CurrentState = newState;
                CurrentState.EnterState();
            }
            else
            {
                Debug.Log($"State not found of type {state.Name}");
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
