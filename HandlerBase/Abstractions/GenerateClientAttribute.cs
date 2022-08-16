using System;

namespace Cblx.Blocks;

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public class GenerateClientAttribute : Attribute
{
    public string? RoutePrefix { get; set; }
}
