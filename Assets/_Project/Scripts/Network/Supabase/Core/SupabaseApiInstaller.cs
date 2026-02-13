using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Network.Supabase.Core
{
    public class SupabaseApiInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("Configure SupabaseApiInstaller");

            builder.Register<SupabaseManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<SessionListener>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            
        }
    }
}