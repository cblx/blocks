namespace Cblx.Blocks.Json.Tests.PrivateConstructors;

#pragma warning disable S3453 // Classes should not have only "private" constructors
public class Person
#pragma warning restore S3453 // Classes should not have only "private" constructors
{
    private Person() { }
    public string Name { get; set; } = "Mary";
    [FlattenJsonProperty]
    public required Address Address { get; set; } = (Address)Activator.CreateInstance(typeof(Address), true)!;
}
