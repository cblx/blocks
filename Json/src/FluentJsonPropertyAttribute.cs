namespace Cblx.Blocks;

public class FluentJsonPropertyAttribute : Attribute
{
    /// <summary>
    /// The inner properties of this property will be flattened into the parent object.
    /// </summary>
    public required bool Flatten { get; set; }
    public Type? ConfigurationType { get; set; }
}

public class FluentJsonPropertyAttribute<T> : FluentJsonPropertyAttribute
    where T : FlattenJsonConfiguration
{
    public FluentJsonPropertyAttribute()
    {
        ConfigurationType = typeof(T);
    }
}