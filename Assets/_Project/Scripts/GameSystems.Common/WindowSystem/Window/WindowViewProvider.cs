using CityBuilder.GameSystems.Common.ViewSystem.ViewsProvider;

namespace CityBuilder.GameSystems.Common.WindowSystem.Window
{
    public class WindowViewProvider : ViewWithModelProvider
    {
        public WindowViewProvider(IViewsProvider viewsProvider) : base(viewsProvider)
        {
        }
    }
}