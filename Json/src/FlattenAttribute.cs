namespace Cblx.Blocks;

[AttributeUsage(AttributeTargets.Property)]
public class FlattenAttribute : Attribute
{
    public Type? ConfigurationType { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public class FlattenAttribute<T> : FlattenAttribute
    where T : FlattenJsonConfiguration
{
    public FlattenAttribute()
    {
        ConfigurationType = typeof(T);
    }
}