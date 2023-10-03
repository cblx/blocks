namespace Cblx.Blocks.Ids.Tests;

[GenerateTypedId]
public class Entity
{
    public EntityId IdDefault { get; init; }
    public EntityId? IdNullable { get; init; }
    public EntityId IdEmpty { get; init; } = EntityId.Empty;
    public EntityId IdValid { get; init; } = EntityId.NewId();
}