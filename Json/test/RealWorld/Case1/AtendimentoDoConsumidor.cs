using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.RealWorld.Case1;
using Cols = TbAtendimento.Cols;
[JsonConverter(typeof(FlattenJsonConverter<AtendimentoDoConsumidor>))]
public abstract class AtendimentoDoConsumidor
{
    [JsonPropertyName(Cols.Id)]
    [JsonInclude]
    public TbId Id { get; protected set; } = TbId.NewId();

    [JsonPropertyName(Cols.TipoDeAtendimento)]
    [JsonInclude]
    public int? TipoDeAtendimento { get; protected set; }

    [JsonPropertyName(Cols.Situacao)]
    [JsonInclude]
    public int? SituacaoDoAtendimento { get; set; }

    [JsonPropertyName(Cols.PermiteInteracao)]
    [JsonInclude]
    public bool? InteracoesAtivas { get; set; }

    [JsonPropertyName(Cols.TipoDeInteracao)]
    [JsonInclude]
    public int? InteracaoAtual { get; set; }

    [JsonPropertyName(Cols.PostoDeAtendimentoId)]
    [JsonInclude]
    public Guid? PostoDeAtendimentoId { get; protected set; }

    [JsonPropertyName(Cols.OrigemAtendimento)]
    [JsonInclude]
    public int? OrigemDoAtendimento { get; protected set; }

    [JsonPropertyName(Cols.ConsumidorId)]
    [JsonInclude]
    public Guid? ConsumidorId { get; protected set; }

    [JsonPropertyName(Cols.DataRespostaDoConsumidor)]
    [JsonInclude]
    public DateTimeOffset? DataDaRespostaDoConsumidor { get; protected set; }

    protected AtendimentoDoConsumidor()
    {
        OrigemDoAtendimento = 0;
        PostoDeAtendimentoId = Guid.NewGuid();
    }
}