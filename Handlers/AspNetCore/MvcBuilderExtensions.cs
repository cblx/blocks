using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace Cblx.Blocks;
public static class MvcBuilderExtensions
{
    public static IMvcBuilder AddHandlers(this IMvcBuilder builder, params Assembly[] assemblies)
    {
         var types = assemblies
            .SelectMany(a => a.DefinedTypes)
            .Where(a => a.GetCustomAttribute<HandlerControllerAttribute>() != null)
            .ToArray();
        return builder.ConfigureApplicationPartManager(m => m.FeatureProviders.Add(new HandlerControllerFeatureProvider(types)));
    }
}
