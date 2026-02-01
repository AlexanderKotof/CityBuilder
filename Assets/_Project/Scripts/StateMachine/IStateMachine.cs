using System;

namespace CityBuilder.StateMachine
{
    public interface IStateMachine<TState>
         where TState : IState
    {
        TState CurrentState { get; }

        void Start(Type startState);

        void Stop();
    }
}