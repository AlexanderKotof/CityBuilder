using GameSystems.Implementation.GameInteraction.InteractionStateMachine.States;
using StateMachine;

namespace GameSystems.Implementation.GameInteraction.InteractionStateMachine
{
    public class PlayerInteractionStateMachine : StateMachineBase<InteractionState>
    {
        public PlayerInteractionStateMachine(params InteractionState[] states) : base(states) { }

        public void Update() => CurrentState?.Update();
    }
}