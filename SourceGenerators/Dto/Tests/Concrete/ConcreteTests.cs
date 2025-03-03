using FluentAssertions;

namespace Cblx.Blocks.SourceGenerators.Dto.Tests.Concrete;

public class ConcreteTests
{
    [Fact]
    public void NullMembersToTest()
    {
        var dto = new NullMembersDto
        {
            Name = "John",
            Age = 30,
        };
        var model = dto.ToModel();
        model.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public void NullMembersFromTest()
    {
        var model = new NullMembersModel
        {
            Name = "John",
            Age = 30,
        };
        var dto = NullMembersDto.From(model);
        dto.Should().BeEquivalentTo(model);
    }

    [Fact]
    public void NotNullMembersTest()
    {
        var dto = new NotNullMembersDto
        {
            Name = "John",
            Age = 30,
        };
        var model = dto.ToModel();
        model.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public void NotNullMembersWithNullTest()
    {
        var dto = new NotNullMembersDto();
        var model = dto.ToModel();
        model.Should().BeEquivalentTo(new { Name = default(string?), Age = default(int) });
    }

    [Fact]
    public void NotNullMembersFromTest()
    {
        var model = new NotNullMembersModel
        {
            Name = "John",
            Age = 30,
        };
        var dto = NotNullMembersDto.From(model);
        dto.Should().BeEquivalentTo(model);
    }

    [Fact]
    public void ModelWithConstructorTest()
    {
        var dto = new ModelWithConstructorDto
        {
            Name = "John",
            Age = 30,
        };
        var model = dto.ToModel();
        model.Should().BeEquivalentTo(dto);
    }
    [Fact]
    public void ModelWithConstructorFromTest()
    {
        var model = new ModelWithConstructor("John", 30);
        var dto = ModelWithConstructorDto.From(model);
        dto.Should().BeEquivalentTo(model);
    }

}

[DtoOf<NullMembersModel>]
internal partial class NullMembersDto
{
    [Mapper] internal partial NullMembersModel ToModel();
    [Mapper] static internal partial NullMembersDto From(NullMembersModel model);
}

class NullMembersModel
{
    public string? Name { get; set; }
    public int? Age { get; set; }
}

[DtoOf<NotNullMembersModel>]
internal partial class NotNullMembersDto
{
    [Mapper] internal partial NotNullMembersModel ToModel();
    [Mapper] static internal partial NotNullMembersDto From(NotNullMembersModel model);
}

class NotNullMembersModel
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; } = 0;
}

[DtoOf<ModelWithConstructor>]
partial class ModelWithConstructorDto
{
    [Mapper] internal partial ModelWithConstructor ToModel();
    [Mapper] static internal partial ModelWithConstructorDto From(ModelWithConstructor model);
}

class ModelWithConstructor
{
    public ModelWithConstructor(string name, int age)
    {
        Name = name;
        Age = age;
    }
    public string Name { get; private set; }
    public int Age { get; private set; }
}