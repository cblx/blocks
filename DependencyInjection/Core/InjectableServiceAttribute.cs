namespace Cblx.Blocks;

public abstract class InjectableServiceAttribute : Attribute
{
    public ServiceLifetime ServiceLifetime { get; }
    public Type? ServiceType { get; }

    protected InjectableServiceAttribute(ServiceLifetime serviceLifetime, Type? serviceType)
    {
        ServiceLifetime = serviceLifetime;
        ServiceType = serviceType;
    }
}