namespace Cblx.Blocks.Json.Tests.Complex_FluentAttributeAnnotated;

public abstract class PersonBase
{
    [FlattenJsonProperty]
    public required Address Address { get; set; }
}