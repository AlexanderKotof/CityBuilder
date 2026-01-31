using GameSystems.Common.ViewSystem.ViewsProvider;

namespace GameSystems.Common.WindowSystem.Window
{
    public class WindowViewProvider : ViewWithModelProvider
    {
        public WindowViewProvider(IViewsProvider viewsProvider) : base(viewsProvider)
        {
        }
    }
}