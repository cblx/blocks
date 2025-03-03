using System;

namespace Cblx.Blocks;

[AttributeUsage(AttributeTargets.Class)]
public class DtoOfAttribute<TSource> : Attribute
{
}
