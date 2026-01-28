using CityBuilder.Dependencies;
using ViewSystem;

namespace GameSystems.Implementation
{
    public class ViewsSystem
    {
        public IViewsProvider ViewsProvider { get; }
        
        public IViewWithModelProvider ViewWithModelProvider { get; }
        
        public ViewsSystem(IViewsProvider viewsProvider, IViewWithModelProvider viewWithModelProvider)
        {
            ViewsProvider = viewsProvider;
            ViewWithModelProvider = viewWithModelProvider;
        }
    }
}