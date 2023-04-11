namespace Cblx.Blocks;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SingletonAttribute<TService> : InjectableServiceAttribute
{
    public SingletonAttribute() : base(ServiceLifetime.Singleton, typeof(TService))
    {
    }
}
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SingletonAttribute : InjectableServiceAttribute
{
    public SingletonAttribute() : base(ServiceLifetime.Singleton, null)
    {
    }
}