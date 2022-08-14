namespace Cblx.Blocks;

[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public class GenerateClientAttribute : Attribute
{
    public string RoutePrefix { get; set; } = string.Empty;
}
