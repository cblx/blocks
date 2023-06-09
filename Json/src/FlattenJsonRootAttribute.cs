using System.Text.Json.Serialization;

namespace Cblx.Blocks;

[AttributeUsage(AttributeTargets.Class)]
public class FlattenJsonRootAttribute : JsonConverterAttribute
{
    public FlattenJsonRootAttribute() : base(typeof(FlattenJsonConverterFactory))
    {
    }

    public Type? ConfigurationType { get; set; }
}

[AttributeUsage(AttributeTargets.Class)]
public class FlattenJsonRootAttribute<T> : FlattenJsonRootAttribute
    where T : FlattenJsonConfiguration
{
    public FlattenJsonRootAttribute()
    {
        ConfigurationType = typeof(T);
    }
}
