SourceGenerator para criar um id tipado para uma class

O atributo `[GenerateTypedId]` gerar um Id tipado struct, readonly e `[ExcludeFromCodeCoverage]`.

A intenção é criar um Id tipado para uma class (Entity) que utiliza um valor único para identificação. (Guid)

Uso:

```csharp
[GenerateTypedId]
public partial class Entity 
{
    // Depois de decorar a class com o atributo [GenerateTypedId]
    // o Id é gerado automaticamente, e pode ser usado da seguinte forma:
    public EntityId Id { get; private set; }
}
```
