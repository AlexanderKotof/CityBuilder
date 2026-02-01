using CityBuilder.Installers;
using VContainer;
using VContainer.Unity;

namespace CityBuilder
{
    /// <summary>
    /// This is entry point of all application, but now it's only marker for main game installer (VContainer, see Scripts/Installers folder) 
    /// </summary>
    public class AppStartup : LifetimeScope
    {
        //TODO: add app level systems, fsm
    
        public GameSystemsInstaller _installer;

        protected override void Configure(IContainerBuilder builder)
        {

        }
    }
}