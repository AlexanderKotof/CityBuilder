using System.Collections.Generic;
using CityBuilder.GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States;
using CityBuilder.StateMachine;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine
{
    public class PlayerInteractionStateMachine : StateMachineBase<InteractionState>, ITickable, IInitializable
    {
        public PlayerInteractionStateMachine(IEnumerable<InteractionState> states) : base(states) { }

        public void Tick() => CurrentState?.Update();
        public void Initialize() => Start(typeof(EmptyInteractionState));
    }
}