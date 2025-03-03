using System;

namespace Cblx.Blocks;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DtoIgnoreAttribute : Attribute
{
    public string Property { get; set; }
}