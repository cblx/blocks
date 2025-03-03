namespace Cblx.Blocks.SourceGenerators.Dto.Tests.Aux;

public class Entity1(string name, int age) : EntityBase
{
    public string Name { get; private set; } = name;
    public int Age { get; private set; } = age;
    public object IgnoreMe { get; private set; } = default!;
}


public class EntityBase
{
    public Guid Id { get; set; }
}