using System.Text.Json.Serialization;

namespace Cblx.Blocks;

[AttributeUsage(AttributeTargets.Class)]
public class FlattenRootAttribute : JsonConverterAttribute
{
    public FlattenRootAttribute() : base(typeof(FlattenJsonConverterFactory))
    {
    }

    public Type? ConfigurationType { get; set; }
}

[AttributeUsage(AttributeTargets.Class)]
public class FlattenRootAttribute<T> : FlattenRootAttribute
    where T : FlattenJsonConfiguration
{
    public FlattenRootAttribute()
    {
        ConfigurationType = typeof(T);
    }
}
