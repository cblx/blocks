using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.RealWorld.Case1;
using Cols = TbAtendimento.Cols;
[JsonConverter(typeof(FlattenJsonConverter<AtendimentoRelacionadoComFornecedor>))]
public class AtendimentoRelacionadoComFornecedor : AtendimentoDoConsumidor
{

    [JsonPropertyName(Cols.TipoDeConsumidor)]
    [JsonInclude]
    public int? TipoDeConsumidor { get; protected set; }

    [JsonPropertyName(Cols.DataDeSolicitacao)]
    [JsonInclude]
    public DateTimeOffset? DataDaSolicitacao { get; protected set; }

    [JsonPropertyName(Cols.FornecedorId)]
    [JsonInclude]
    public TbId? FornecedorId { get; protected set; }

    [Flatten<ClassificacaoFlattenConfiguration>]
    public Classificacao Classificacao { get; protected set; } = Classificacao.Empty();
    
    [JsonPropertyName(Cols.AtendimentoAnteriorId)]
    [JsonInclude]
    public Guid? AtendimentoAnteriorId { get; protected set; }

    [JsonPropertyName(Cols.EventoEspecialId)]
    [JsonInclude]
    public Guid? ControleDePrazosId { get; protected set; }

    [JsonPropertyName(Cols.TipoDeConversaoDisponivel)]
    [JsonInclude]
    public int? ConversaoDisponivel { get; private set; }

    [JsonPropertyName(Cols.EncaminharAoDepartamentoDeFiscalizacao)]
    [JsonInclude]
    public bool? EncaminharAoDepartamentoDeFiscalizacao { get; private set; }

    [JsonPropertyName(Cols.RespostaDoConsumidor)]
    [JsonInclude]
    public int? RespostaDoConsumidor { get; protected set; }

    [JsonPropertyName(Cols.StateCode)]
    [JsonInclude]
    public int? StateCode { get; protected set; }

    public void DesativarInteracoes()
    {
        InteracoesAtivas = false;
    }
}

