namespace Cblx.Blocks.SourceGenerators.Dto;

internal record DtoExtractionModel
{
    public string Namespace { get; set; } = default!;
    public string ClassName { get; set; } = default!;
    public EquatableArray<string> Properties { get; set; } = default!;
    public EquatableArray<MapperExtractionModel> Mappers { get; set; } = default!;
}
