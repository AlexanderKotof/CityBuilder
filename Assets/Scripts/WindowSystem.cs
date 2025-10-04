using CityBuilder.Dependencies;
using GameSystems;
using ViewSystem;

public class WindowSystem : GameSystemBase
{
    private readonly IViewWithModelProvider _viewWithModelProvider;
    
    public WindowsProvider WindowsProvider { get; }

    public WindowSystem(IDependencyContainer container) : base(container)
    {
        WindowsProvider = new WindowsProvider(container.Resolve<IViewWithModelProvider>());
    }
}