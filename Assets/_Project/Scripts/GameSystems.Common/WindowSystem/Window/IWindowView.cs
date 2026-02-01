using System;

namespace CityBuilder.GameSystems.Common.WindowSystem.Window
{
    public interface IWindowView
    {
        internal Type ViewModelType { get; }
    }
}