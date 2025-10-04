using CityBuilder.Dependencies;

namespace ViewSystem
{
    public class WindowViewProvider : ViewWithModelProvider
    {
        public WindowViewProvider(IViewsProvider viewsProvider) : base(viewsProvider)
        {
        }
    }
}