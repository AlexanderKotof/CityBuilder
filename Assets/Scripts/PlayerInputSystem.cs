using System.Threading.Tasks;
using GameSystems;
using PlayerInput;

public class PlayerInputSystem : IGameSystem, IUpdateGamSystem
{
    public PlayerInputManager PlayerInputManager { get; } = new();
    
    public Task Init()
    {
        return Task.CompletedTask;
    }

    public Task Deinit()
    {
        return Task.CompletedTask;
    }

    public void Update()
    {
        PlayerInputManager.Update();
    }
}