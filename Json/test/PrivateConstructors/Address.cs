namespace Cblx.Blocks.Json.Tests.PrivateConstructors;

#pragma warning disable S3453 // Classes should not have only "private" constructors
public class Address
#pragma warning restore S3453 // Classes should not have only "private" constructors
{
    private Address() { }
    public string Street { get; set; } = "Elm Street";
}