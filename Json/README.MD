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
---

### Um tipo, diferentes mapeamentos
```csharp
public class Pessoa 
{
    [JsonPropertyName("nome")]
    public string Nome { get; set; }
    [JsonFlatten<EnderecoResidencialConfiguration>]
    public Endereco EnderecoResidencial { get; set; }
    [JsonFlatten<EnderecoComercialConfiguration>]
    public Endereco EnderecoComercial { get; set; }
}

public class Endereco 
{
    public string Logradouro { get; set; }
    public string Numero { get; set; }
}

public class EnderecoResidencialConfiguration : JsonFlattenConfiguration<Endereco> 
{
    public EnderecoResidencialConfiguration()
    {
        HasPropertyName(e => e.Logradouro, "residencial_logradouro");
        HasPropertyName(e => e.Numero, "residencial_numero");
    }
}

public class EnderecoComercialConfiguration : JsonFlattenConfiguration<Endereco> 
{
    public EnderecoComercialConfiguration()
    {
        HasPropertyName(e => e.Logradouro, "comercial_logradouro");
        HasPropertyName(e => e.Numero, "comercial_numero");
    }
}
```
Ao serializar, gera:
```json
{
    "nome": "João",
    "residencial_logradouro": "Rua X",
    "residencial_numero": "123",
    "comercial_logradouro": "Rua X",
    "comercial_numero": "123"
}
```
