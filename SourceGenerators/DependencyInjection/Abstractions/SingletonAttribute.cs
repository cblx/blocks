namespace Cblx.Blocks;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SingletonAttribute<TService> : Attribute { }
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SingletonAttribute : Attribute { }