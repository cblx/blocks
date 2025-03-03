namespace Cblx.Blocks.SourceGenerators.Dto;

internal record MapperExtractionModel
{
    public string Namespace { get; set; } = default!;
    public string ClassName { get; set; } = default!;
    public string MethodName { get; set; } = default!;
    public string ReturnTypeName { get; set; } = default!;
    public string MethodModifiers { get; set; } = default!;
    //public bool ShouldGenerate { get; set; }
    public string SourceName { get; set; } = default!;
    public string SourceType { get; set; } = default!;
    public EquatableArray<Argument> ConstructorParameters { get; set; } = default!;
    public EquatableArray<Argument> SourceProperties { get; set; } = default!;
    public EquatableArray<Argument> TargetProperties { get; set; } = default!;

    //public EquatableArray<string> Properties { get; set; } = default!;
}
