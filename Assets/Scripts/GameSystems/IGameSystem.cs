namespace GameSystems
{
    public interface IGameSystem
    {
        void Init();
        void Deinit();
    }

    public interface IUpdateGamSystem
    {
        void Update();
    }
}