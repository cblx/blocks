namespace Cblx.Blocks.Json.Tests.FluentIgnoreWithRootConfiguration;

[FlattenJsonRoot<PersonConfiguration>]
public class Person
{
    public required string Name { get; set; }
    [FlattenJsonProperty<AddressConfiguration>]
    public required AddressVo Address { get; set; }
}
