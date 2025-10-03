using System;
using UnityEngine;

namespace StateMachine
{
    public abstract class StateBase : IState
    {
        public event Action<Type> ChangeStateRequested;

        public void ChangeState(Type state)
        {
            ChangeStateRequested?.Invoke(state);
        }

        public void ChangeState<TState>() where TState : IState
        {
            ChangeStateRequested?.Invoke(typeof(TState));
        }

        public void EnterState()
        {
            Debug.Log($"Entering state {GetType().Name}");
            OnEnterState();
        }

        public void ExitState()
        {
            Debug.Log($"Exiting state {GetType().Name}");
            OnExitState();
        }

        protected abstract void OnEnterState();
        protected abstract void OnExitState();
    }
}
