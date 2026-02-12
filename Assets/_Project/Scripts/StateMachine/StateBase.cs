using System;
using Network.Supabase.Core;

namespace CityBuilder.StateMachine
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
            Logger.Log($"Entering state {GetType().Name}");
            OnEnterState();
        }

        public void ExitState()
        {
            Logger.Log($"Exiting state {GetType().Name}");
            OnExitState();
        }

        protected abstract void OnEnterState();
        protected abstract void OnExitState();
    }
}
