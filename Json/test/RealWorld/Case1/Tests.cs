using System.Text.Json;

namespace Cblx.Blocks.Json.Tests.RealWorld.Case1;

public class Tests
{
    private const string _jsonSubject = """
        {"@odata.context":"https://proconspdev.api.crm2.dynamics.com/api/data/v9.0/$metadata#procon_atendimentos(procon_produto_servico,procon_data_compra_aquisicao,procon_descricao_consulta,procon_descricao_primeiro_contato,procon_nome_empresa,procon_tipo_consumidor,procon_data_solicitacao_fase1,_procon_fornecedor_value,_procon_area_value,_procon_assunto_value,_procon_grupo_problema_value,_procon_problema_value,_procon_atendimento_anterior_value,_procon_controle_prazo_value,procon_converter_atendimento,procon_encaminhar_dfisc,procon_resposta_consumidor,statecode,procon_atendimentoid,procon_tipo_atendimento,statuscode,procon_status_interacao,procon_interacao,_procon_posto_atendimento_value,procon_origem_atendimento,_procon_consumidor_value,procon_data_resposta_consumidor)/$entity","@odata.etag":"W/\"220903320\"","procon_produto_servico":"aaaaa","procon_data_compra_aquisicao":"2023-06-01T03:00:00Z","procon_descricao_consulta":"aweeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee","procon_descricao_primeiro_contato":null,"procon_nome_empresa":null,"procon_tipo_consumidor":472890000,"procon_data_solicitacao_fase1":"2023-06-01T15:08:56Z","_procon_fornecedor_value":"4edb0124-f7a2-eb11-b1ac-002248374d69","_procon_area_value":null,"_procon_assunto_value":null,"_procon_grupo_problema_value":null,"_procon_problema_value":null,"_procon_atendimento_anterior_value":null,"_procon_controle_prazo_value":"489390f1-892d-eb11-a813-000d3ac179a6","procon_converter_atendimento":6,"procon_encaminhar_dfisc":false,"procon_resposta_consumidor":null,"statecode":0,"procon_atendimentoid":"82b11ca8-0eab-426a-b54a-fcc95fcfb919","procon_tipo_atendimento":1,"statuscode":472890000,"procon_status_interacao":true,"procon_interacao":2,"_procon_posto_atendimento_value":"f600a8e3-922e-eb11-a813-000d3ac179a6","procon_origem_atendimento":4,"_procon_consumidor_value":"7ec26c95-f8f7-4e18-b1d8-cec56bd1bddc","procon_data_resposta_consumidor":null,"_organizationid_value":"0ee4a0d6-643a-45b0-89aa-3b3e3b01733d"}
        """;

    //private readonly Consulta _subject = new Consulta(
    //    );

    //[Fact]
    //public void Serializing()
    //{
    //    Assert.Equal(_jsonSubject, JsonSerializer.Serialize(_personSubject, Fixtures.CreateOptions<Person>()));
    //}

    [Fact]
    public void Deserializing()
    {
        var consulta = JsonSerializer.Deserialize<Consulta>(_jsonSubject);
        Assert.Equal("aweeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee", consulta.DescricaoDaConsulta);
    }
}
