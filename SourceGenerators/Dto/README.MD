SourceGenerator para adicionar a uma classe um construtor sem parâmetros.

O atributo `[HasObsoleteEmptyConstructor]` gerar um construtor público e marcado como `[Obsolete]` e `[ExcludeFromCodeCoverage]`.

O atributo `[HasPrivateEmptyConstructor]` gerar um construtor privado e marcado `[ExcludeFromCodeCoverage]`.

A intenção é utilizar em classes que são usadas na serialização do Sytem.Text.Json, mas que não seria desejável ter um construtor vazio.

Uso:

```csharp
[HasObsoleteEmptyConstructor]
public partial class MinhaClasseComConstrutorPublicoObsoleto {
}

[HasPrivateEmptyConstructor]
public partial class MinhaClasseComConstrutorPrivado {
}
```
