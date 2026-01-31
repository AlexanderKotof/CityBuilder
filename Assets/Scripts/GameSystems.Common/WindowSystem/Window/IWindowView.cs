using System;

namespace GameSystems.Common.WindowSystem.Window
{
    public interface IWindowView
    {
        internal Type ViewModelType { get; }
    }
}