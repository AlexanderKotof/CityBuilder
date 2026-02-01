namespace GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine.States
{
    public enum CursorStateEnum
    {
        None = 0,
        Selection,
        Hover,
        
        Accepted,
        Rejected,
        Upgrade,
        Merge,
    }
}