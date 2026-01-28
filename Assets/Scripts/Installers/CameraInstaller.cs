using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Installers
{
    public class CameraInstaller : LifetimeScope
    {
        public Camera RaycasterCamera;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(RaycasterCamera).As<Camera>();
        }
    }
}