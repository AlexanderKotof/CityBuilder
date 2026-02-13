using CityBuilder.GameSystems.Common.ViewSystem.ViewsProvider;
using CityBuilder.GameSystems.Common.WindowSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Network.Supabase.Core
{
    public class ViewSystemInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("Configure ViewSystemInstaller");
           
        }
    }
}