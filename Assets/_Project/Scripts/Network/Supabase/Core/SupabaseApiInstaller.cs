using CityBuilder.Network.SupabaseApi;
using VContainer;
using VContainer.Unity;

namespace Network.Supabase.Core
{
    public class SupabaseApiInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentOnNewGameObject<SupabaseManager>(Lifetime.Singleton, nameof(SupabaseManager))
                .AsSelf().AsImplementedInterfaces();
			
            builder.Register<SessionListener>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<GuestAuthService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
    }
}