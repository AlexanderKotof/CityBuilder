using System.Threading.Tasks;

namespace GameSystems
{
    public interface IGameSystem
    {
        Task Init();
        Task Deinit();
    }
    
    public interface IUpdateGamSystem
    {
        void Update();
    }
}