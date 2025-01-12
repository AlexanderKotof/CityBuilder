namespace CityBuilder.Dependencies
{
    public interface IDependencyContainer
    {
        void Register<T>(T value);
        T Resolve<T>();
    }
}