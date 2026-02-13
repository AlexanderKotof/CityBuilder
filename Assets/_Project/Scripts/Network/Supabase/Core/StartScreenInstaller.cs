using CityBuilder.GameSystems.Common.ViewSystem.ViewsProvider;
using CityBuilder.GameSystems.Common.WindowSystem;
using CityBuilder.GameSystems.Implementation.HudWindow;
using CityBuilder.Network.SupabaseApi;
using VContainer;
using VContainer.Unity;

namespace Network.Supabase.Core
{
    public class CommonAppSystemsInstaller : IInstaller
    {
        public void Install(IContainerBuilder builder)
        {
            builder.Register<ViewsProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ViewWithModelProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<WindowsProvider>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            builder.Register<SupabaseManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<SessionListener>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            
            builder.Register<GuestAuthService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<GameSceneLoader>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<StartWindowFeature>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
    }
    
    public class StartScreenInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GuestAuthService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<GameSceneLoader>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<StartWindowFeature>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
    }
}