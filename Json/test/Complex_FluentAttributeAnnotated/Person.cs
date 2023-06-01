using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.Complex_FluentAttributeAnnotated;
[JsonConverter(typeof(FlattenJsonConverter<Person>))]
public class Person : PersonBase
{
#pragma warning disable CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.
    private Person() { }
#pragma warning restore CS8618 // O campo não anulável precisa conter um valor não nulo ao sair do construtor. Considere declará-lo como anulável.

    [JsonPropertyName("my_name")]
    public string Name { get; private set; }
    public int Age { get; private set; }
 
    public Person WithValues(string name, int age, string street)
    {
        Name = name;
        Age = age;
        Address = new Address { Street = street };
        return this;
    }
}
