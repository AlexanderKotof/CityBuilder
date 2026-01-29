using CityBuilder.Dependencies;
using GameSystems.Common.ViewSystem.ViewsProvider;

namespace ViewSystem
{
    public class WindowViewProvider : ViewWithModelProvider
    {
        public WindowViewProvider(IViewsProvider viewsProvider) : base(viewsProvider)
        {
        }
    }
}