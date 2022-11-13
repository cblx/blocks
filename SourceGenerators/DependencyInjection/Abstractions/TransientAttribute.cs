namespace Cblx.Blocks;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class TransientAttribute<TService> : Attribute { }
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class TransientAttribute : Attribute { }