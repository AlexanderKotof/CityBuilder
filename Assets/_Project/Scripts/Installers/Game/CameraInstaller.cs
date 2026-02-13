using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace CityBuilder.Installers
{
    public class CameraInstaller : LifetimeScope
    {
        public Transform CameraRoot;
        public Camera Camera;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(CameraRoot).Keyed(nameof(CameraRoot)).AsSelf();
            builder.RegisterInstance(Camera).As<Camera>();
        }
    }
}