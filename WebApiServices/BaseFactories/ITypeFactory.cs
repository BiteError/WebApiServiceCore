namespace WebApiServices.BaseFactories
{
    public interface ITypeFactory : IFactory
    {
        T Create<T>() where T : class;
    }
}