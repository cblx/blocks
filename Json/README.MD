# Utilitários para serialização Json utilizando o System.Text.Json

## JsonFlatten

### Uso básico

```csharp
public class Pessoa 
{
    public string Nome { get; set; }
    [JsonFlatten]
    public Endereco Endereco { get; set; }
}

public class Endereco 
{
    public string Logradouro { get; set; }
    public string Numero { get; set; }
}
```
Ao serializar, gera:
```json
{
    "Nome": "João",
    "Logradouro": "Rua X",
    "Numero": "123"
}
```