using CityBuilder.Dependencies;

namespace GameSystems
{
    public abstract class GameSystemBase : IGameSystem
    {
        protected IDependencyContainer Container { get; }

        protected GameSystemBase(IDependencyContainer container)
        {
            Container = container;
        }
    
        public abstract void Init();
        public abstract void Deinit();
        public abstract void Update();
    }
}