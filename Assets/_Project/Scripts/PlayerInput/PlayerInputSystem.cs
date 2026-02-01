using VContainer.Unity;

namespace CityBuilder.PlayerInput
{
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
}