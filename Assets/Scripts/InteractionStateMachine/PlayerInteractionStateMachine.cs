using StateMachine;

namespace InteractionStateMachine
{
    public class PlayerInteractionStateMachine : StateMachineBase<InteractionState>
    {
        public PlayerInteractionStateMachine(params InteractionState[] states) : base(states) { }

        public void Update() => CurrentState?.Update();
    }
}