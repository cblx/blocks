namespace Cblx.Blocks;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ScopedAttribute<TService> : InjectableServiceAttribute
{
    public ScopedAttribute() : base(ServiceLifetime.Scoped, typeof(TService))
    {
    }
}
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ScopedAttribute : InjectableServiceAttribute
{
    public ScopedAttribute() : base(ServiceLifetime.Scoped, null)
    {
    }
}