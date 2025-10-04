using CityBuilder.Dependencies;
using ViewSystem;

namespace GameSystems.Implementation
{
    public class ViewsSystem : GameSystemBase
    {
        public IViewsProvider ViewsProvider { get; }
        
        public IViewWithModelProvider ViewWithModelProvider { get; }
        
        public ViewsSystem(IDependencyContainer container) : base(container)
        {
            ViewsProvider = new ViewsProvider();
            ViewWithModelProvider = new ViewWithModelProvider(ViewsProvider);
        }
    }
}