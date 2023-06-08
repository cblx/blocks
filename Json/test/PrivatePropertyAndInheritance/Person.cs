//using System.Text.Json.Serialization;

//namespace Cblx.Blocks.Json.Tests.PrivatePropertyAndInheritance;

//public class Person : PersonBase
//{
//    [JsonPropertyName("name")]
//    public string Name { get; private set; } = "Mary";
//    [Flatten]
//    private Address Address { get; set; } = new();
//    public Address GetAddress() => Address;
//}


//public class PersonBase : PersonCore
//{
//    public string BaseProperty { get; private set; } = "BaseProperty";
//}

//public class PersonCore
//{

//}