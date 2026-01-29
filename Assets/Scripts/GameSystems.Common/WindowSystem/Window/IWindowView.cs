using System;

namespace ViewSystem
{
    public interface IWindowView
    {
        internal Type ViewModelType { get; }
    }
}