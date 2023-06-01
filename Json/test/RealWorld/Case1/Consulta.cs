using System.Text.Json.Serialization;

namespace Cblx.Blocks.Json.Tests.RealWorld.Case1;
using Cols = TbAtendimento.Cols;
[JsonConverter(typeof(FlattenJsonConverter<Consulta>))]
public partial class Consulta : AtendimentoRelacionadoComFornecedor
{
    [JsonPropertyName(Cols.ProdutoOuServicoDaConsulta)]
    [JsonInclude]
    public string? ProdutoOuServicoContratado { get; private set; }

    [JsonPropertyName(Cols.DataDaAquisicaoNaConsulta)]
    [JsonInclude]
    public DateTimeOffset? DataDaCompraAquisicao { get; private set; }

    [JsonPropertyName(Cols.DescricaoDaConsulta)]
    [JsonInclude]
    public string? DescricaoDaConsulta { get; private set; }

    // Foi solicitado por analista para fazer a cópia do campo para a reclamação.
    // por isso o campo existe aqui.
    // Mas parece não fazer sentido pois na criação de consulta isso nunca é pedido.
    [JsonPropertyName(Cols.DescricaoDoPrimeiroContatoComFornecedor)]
    [JsonInclude]
    public string? ComoFoiOContatoComOFornecedor { get; private set; }

    [JsonPropertyName(Cols.RascunhoDeFornecedor_RazaoSocial)]
    [JsonInclude]
    public string? RascunhoFornecedorNome { get; private set; }

    public Consulta() { }

    public Consulta(
        TbId atendimentoId,
        Guid consumidorId,
        TbId? fornecedorId,
        Guid? consultaOriginalId,
        string? nomeDoFornecedorNaoEncontrado,
        Classificacao classificacao,
        DateTimeOffset? dataDaCompraAquisicao,
        string? produtoOuServicoContratado,
        string? descricaoDaConsulta
    )
    {
        Id = atendimentoId;

        ControleDePrazosId = Guid.NewGuid();
        DataDaSolicitacao = DateTimeOffset.Now;
        TipoDeAtendimento = 0;
        TipoDeConsumidor = 0;
        SituacaoDoAtendimento = 0;

        ConsumidorId = consumidorId;
        FornecedorId = fornecedorId;
        RascunhoFornecedorNome = nomeDoFornecedorNaoEncontrado;
        AtendimentoAnteriorId = consultaOriginalId;
        Classificacao = classificacao;
        DataDaCompraAquisicao = dataDaCompraAquisicao;
        ProdutoOuServicoContratado = produtoOuServicoContratado;
        DescricaoDaConsulta = descricaoDaConsulta;
    }

    public void Cancelar()
    {
        RespostaDoConsumidor = 0;
        SituacaoDoAtendimento = 0;
        DataDaRespostaDoConsumidor = DateTimeOffset.Now;
        StateCode = 0;
    }

    public void SinalizarResolucao()
    {
        RespostaDoConsumidor = 0;
        SituacaoDoAtendimento = 0;
        DataDaRespostaDoConsumidor = DateTimeOffset.Now;
        StateCode = 0;
    }
}