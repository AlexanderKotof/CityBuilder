
using System;

namespace CityBuilder.StateMachine
{
    public interface IState
    {
        event Action<Type> ChangeStateRequested;

        void EnterState();

        void ExitState();

        void ChangeState(Type state);

        void ChangeState<TState>() where TState : IState;
    }
}
