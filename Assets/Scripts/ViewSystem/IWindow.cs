using System;

namespace ViewSystem
{
    public interface IWindow
    {
        internal Type ViewModelType { get; }
    }
}