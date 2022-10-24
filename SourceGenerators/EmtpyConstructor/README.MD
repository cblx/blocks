SourceGenerator para adicionar a uma classe um construtor sem parâmetros.

O construtor gerado é público e marcado como `[Obsolete]` e `[ExcludeFromCodeCoverage]`.

A intenção é utilizar em classes que são usadas na serialização do Sytem.Text.Json, mas que não seria desejável ter um construtor vazio.

Futuramente pode haver uma opção para gerar um construtor privado. Dependerá de no .NET7 estar prático de customizar a serialização para aceitar construtor privado.

Uso:

```csharp
[HasObsoleteConstructor]
public partial class MinhaClasse {
}
```
