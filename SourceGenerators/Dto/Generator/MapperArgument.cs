namespace Cblx.Blocks.SourceGenerators.Dto;

internal record Argument
{
    public string Name { get; set; } = default!;
    public bool Nullable { get; set; }
    public bool IsReferenceType { get; set; }
}