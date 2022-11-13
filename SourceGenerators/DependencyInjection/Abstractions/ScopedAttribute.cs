namespace Cblx.Blocks;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ScopedAttribute<TService> : Attribute { }
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ScopedAttribute : Attribute { }