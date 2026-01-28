using System.Collections.Generic;
using GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States;
using StateMachine;
using VContainer.Unity;

namespace GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine
{
    public class PlayerInteractionStateMachine : StateMachineBase<InteractionState>, ITickable, IInitializable
    {
        public PlayerInteractionStateMachine(IEnumerable<InteractionState> states) : base(states) { }

        public void Tick() => CurrentState?.Update();
        public void Initialize() => Start(typeof(EmptyInteractionState));
    }
}