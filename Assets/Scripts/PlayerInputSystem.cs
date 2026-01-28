using PlayerInput;
using VContainer.Unity;

public class PlayerInputSystem : ITickable
{
    public PlayerInputSystem(PlayerInputManager playerInputManager)
    {
        PlayerInputManager = playerInputManager;
    }

    public PlayerInputManager PlayerInputManager { get; }
    
    public void Tick()
    {
        PlayerInputManager.Update();
    }
}