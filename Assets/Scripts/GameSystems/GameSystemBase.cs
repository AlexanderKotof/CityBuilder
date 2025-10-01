using System.Threading.Tasks;
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

        public virtual Task Init()
        {
            return Task.CompletedTask;
        }

        public virtual Task Deinit()
        {
            return Task.CompletedTask;
        }
    }
}