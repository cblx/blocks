namespace Cblx.Blocks.Json.Tests.Complex_FluentAttributeAnnotated;

public abstract class PersonBase
{
    [Flatten]
    public required Address Address { get; set; }
}