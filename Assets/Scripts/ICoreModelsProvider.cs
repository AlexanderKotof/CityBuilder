public interface ICoreModelsProvider
{
    public T GetModel<T>() where T : class;
    public T RegisterModel<T>(T instance) where T : class;
}