using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.RealWorld.Case1;

public class Tests
{
    private const string _jsonSubject = """
        {"@odata.context":"https://host.dynamics.com/api/data/v9.0/$metadata#app_atendimentos(app_produto_servico,app_data_compra_aquisicao,app_descricao_consulta,app_descricao_primeiro_contato,app_nome_empresa,app_tipo_consumidor,app_data_solicitacao_fase1,_app_fornecedor_value,_app_area_value,_app_assunto_value,_app_grupo_problema_value,_app_problema_value,_app_atendimento_anterior_value,_app_controle_prazo_value,app_converter_atendimento,app_encaminhar_dfisc,app_resposta_consumidor,statecode,app_atendimentoid,app_tipo_atendimento,statuscode,app_status_interacao,app_interacao,_app_posto_atendimento_value,app_origem_atendimento,_app_consumidor_value,app_data_resposta_consumidor)/$entity","@odata.etag":"W/\"220903320\"","app_produto_servico":"aaaaa","app_data_compra_aquisicao":"2023-06-01T03:00:00Z","app_descricao_consulta":"aweeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee","app_descricao_primeiro_contato":null,"app_nome_empresa":null,"app_tipo_consumidor":472890000,"app_data_solicitacao_fase1":"2023-06-01T15:08:56Z","_app_fornecedor_value":"4edb0124-f7a2-eb11-b1ac-002248374d69","_app_area_value":null,"_app_assunto_value":null,"_app_grupo_problema_value":null,"_app_problema_value":null,"_app_atendimento_anterior_value":null,"_app_controle_prazo_value":"489390f1-892d-eb11-a813-000d3ac179a6","app_converter_atendimento":6,"app_encaminhar_dfisc":false,"app_resposta_consumidor":null,"statecode":0,"app_atendimentoid":"82b11ca8-0eab-426a-b54a-fcc95fcfb919","app_tipo_atendimento":1,"statuscode":472890000,"app_status_interacao":true,"app_interacao":2,"_app_posto_atendimento_value":"f600a8e3-922e-eb11-a813-000d3ac179a6","app_origem_atendimento":4,"_app_consumidor_value":"7ec26c95-f8f7-4e18-b1d8-cec56bd1bddc","app_data_resposta_consumidor":null,"_organizationid_value":"0ee4a0d6-643a-45b0-89aa-3b3e3b01733d"}
        """;

    [Fact]
    public void Deserializing()
    {
        var consulta = JsonSerializer.Deserialize<Consulta>(_jsonSubject)!;
        Assert.Equal(DateTimeOffset.Parse("2023-06-01T03:00:00Z"), consulta.DataDaCompraAquisicao);
        Assert.Equal("aaaaa", consulta.ProdutoOuServicoContratado);
        Assert.Equal("aweeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee", consulta.DescricaoDaConsulta);
        Assert.Equal(new TbId("4edb0124-f7a2-eb11-b1ac-002248374d69"), consulta.FornecedorId);
        Assert.Equal(new TbId("82b11ca8-0eab-426a-b54a-fcc95fcfb919"), consulta.Id);
        Assert.Equal(new TbId("489390f1-892d-eb11-a813-000d3ac179a6"), consulta.ControleDePrazosId);
    }
}
