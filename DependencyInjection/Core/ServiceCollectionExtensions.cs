using System.Reflection;
namespace Cblx.Blocks;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAnnotatedServicesUsingReflection(this IServiceCollection services, params Assembly[] assemblies)
    {
        var types = assemblies.SelectMany(a => a.DefinedTypes);
        var serviceTypes = from t in types
                       from attr in t.GetCustomAttributes(true).Where(attr => attr is InjectableServiceAttribute)
                       select new
                       {
                           Type = t,
                           Attribute = attr as InjectableServiceAttribute
                       };
        serviceTypes
            .ToList()
            .ForEach(s => services.Add(new ServiceDescriptor(s.Attribute.ServiceType ?? s.Type, s.Type, s.Attribute.ServiceLifetime)));
        return services;
    }
}
