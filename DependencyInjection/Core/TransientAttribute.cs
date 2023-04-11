namespace Cblx.Blocks;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class TransientAttribute<TService> : InjectableServiceAttribute
{
    public TransientAttribute() : base(ServiceLifetime.Transient, typeof(TService))
    {
    }
}
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class TransientAttribute : InjectableServiceAttribute
{
    public TransientAttribute() : base(ServiceLifetime.Transient, null)
    {
    }
}