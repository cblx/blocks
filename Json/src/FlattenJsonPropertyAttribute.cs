namespace Cblx.Blocks;

[AttributeUsage(AttributeTargets.Property)]
public class FlattenJsonPropertyAttribute : Attribute
{
    public Type? ConfigurationType { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public class FlattenJsonPropertyAttribute<T> : FlattenJsonPropertyAttribute
    where T : FlattenJsonConfiguration
{
    public FlattenJsonPropertyAttribute()
    {
        ConfigurationType = typeof(T);
    }
}
