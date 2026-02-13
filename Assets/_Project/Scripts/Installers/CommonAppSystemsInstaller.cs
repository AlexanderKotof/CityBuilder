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
            InstallViewsSystem(builder);
            InstallNetworkClient(builder);
            InstallSharedServices(builder);
            InstallStartScreen(builder);
        }
        
        private static void InstallViewsSystem(IContainerBuilder builder)
        {
            builder.Register<ViewsProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ViewWithModelProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<WindowsProvider>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }

        private static void InstallNetworkClient(IContainerBuilder builder)
        {
            builder.Register<SupabaseManager>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<SessionListener>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
        
        private static void InstallSharedServices(IContainerBuilder builder)
        {
            builder.Register<GuestAuthService>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
            builder.Register<GameSceneLoader>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }

        private static void InstallStartScreen(IContainerBuilder builder)
        {
            builder.Register<StartWindowFeature>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        }
    }
}