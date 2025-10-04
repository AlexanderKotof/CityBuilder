using GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States;
using StateMachine;

namespace GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine
{
    public class PlayerInteractionStateMachine : StateMachineBase<InteractionState>
    {
        public PlayerInteractionStateMachine(params InteractionState[] states) : base(states) { }

        public void Update() => CurrentState?.Update();
    }
}