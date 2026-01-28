using CityBuilder.Dependencies;
using GameSystems;
using ViewSystem;

public class WindowSystem
{
    public WindowsProvider WindowsProvider { get; }

    public WindowSystem(WindowsProvider provider)
    {
        WindowsProvider = provider;
    }
}