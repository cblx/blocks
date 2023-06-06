namespace Cblx.Blocks.Json.Tests.RealWorld.Case2;

using System.Text.Json.Serialization;
using Cols = TbClientePotencial.Cols;

[FlattenRoot]
public partial class ClientePotencial 
{
    public ClientePotencial() { }

    [JsonPropertyName(Cols.Id)]
    public Guid Id { get; private set; }

    [JsonPropertyName(Cols.Cpf)]
    public string? Cpf { get; private set; }

    [JsonPropertyName(Cols.CpfConfirmado)]
    public int? CpfConfirmado { get; private set; }

    [JsonPropertyName($"{Cols.CpfConfirmado}@x")]
    public string? CpfConfirmadoFormatted { get; private set; }

    [JsonPropertyName(Cols.NomeCompletoDoBeneficiarioDoAuxilio)]
    public string? NomeCompleto { get; private set; }

    [JsonPropertyName(Cols.ProjetoDeOrigem)]
    public string? ProjetoDeOrigem { get; private set; }

    [JsonPropertyName(Cols.TipoDeAtendimento)]
    public int? TipoDeAtendimento { get; private set; }

    [JsonPropertyName(Cols.Rg)]
    public string? NumeroDaIdentidade { get; private set; }

    [JsonPropertyName(Cols.RgOuRne)]
    public int? RgRne { get; private set; }

    [JsonPropertyName(Cols.UfRg)]
    public int? UfRg { get; private set; }

    [JsonPropertyName(Cols.Sexo)]
    public int? Sexo { get; private set; }

    [JsonPropertyName(Cols.NomeDaMae)]
    public string? NomeDaMae { get; private set; }

    [JsonPropertyName(Cols.DataDeNascimento)]
    public DateOnly? DataDeNascimento { get; private set; }

    [JsonPropertyName(Cols.Idade)]
    public int? Idade { get; private set; }

    [JsonPropertyName(Cols.CidadeDeNascimento)]
    public string? MunicipioDeNascimento { get; private set; }

    [JsonPropertyName(Cols.UfDeNascimento)]
    public string? UfDeNascimento { get; private set; }

    [JsonPropertyName(Cols.Nacionalidade)]
    public string? Nacionalidade { get; private set; }

    [JsonPropertyName(Cols.CorRaca)]
    public int? RacaCor { get; private set; }

    [JsonPropertyName(Cols.Logradouro)]
    public string? Logradouro { get; private set; }

    [JsonPropertyName(Cols.Complemento)]
    public string? Complemento { get; private set; }

    [JsonPropertyName(Cols.Bairro)]
    public string? Bairro { get; private set; }

    [JsonPropertyName(Cols.Cidade)]
    public string? Cidade { get; private set; }

    [JsonPropertyName(Cols.Uf)]
    public string? Uf { get; private set; }

    [JsonPropertyName(Cols.Pais)]
    public string? Pais { get; private set; }

    [JsonPropertyName(Cols.LocalizacaoDaMoradia)]
    public int? LocalizacaoDaMoradia { get; private set; }

    [JsonPropertyName(Cols.Cep)]
    public string? Cep { get; private set; }

    [JsonPropertyName(Cols.OpcaoMunicipio1Id)]
    public Guid? MunicipioOpcao1Id { get; private set; }

    [JsonPropertyName(Cols.OpcaoMunicipio2Id)]
    public Guid? MunicipioOpcao2Id { get; private set; }

    [JsonPropertyName(Cols.OpcaoMunicipio3Id)]
    public Guid? MunicipioOpcao3Id { get; private set; }

    [JsonPropertyName(Cols.OpcaoZona1)]
    public int? ZonaParaMunicipio1 { get; private set; }

    [JsonPropertyName(Cols.OpcaoZona2)]
    public int? ZonaParaMunicipio2 { get; private set; }

    [JsonPropertyName(Cols.OpcaoZona3)]
    public int? ZonaParaMunicipio3 { get; private set; }

    [JsonPropertyName(Cols.TipoDeLogradouro)]
    public int? TipoDeLogradouro { get; private set; }

    [JsonPropertyName(Cols.TipoDeMoradia)]
    public int? TipoDeMoradia { get; private set; }

    [JsonPropertyName(Cols.TempoDeMoradiaNoEstadoDeSPEmAnos)]
    public int? TempoDeMoradiaNoEstadoDeSp { get; private set; }

    [JsonPropertyName(Cols.TempoDeMoradiaNoMunicipioDeSPEmAnos)]
    public int? TempoDeMoradiaNoMunicipioDeSp { get; private set; }

    [JsonPropertyName(Cols.Email)]
    public string? Email { get; private set; }

    [JsonPropertyName(Cols.Celular)]
    public string? Celular { get; private set; }

    [JsonPropertyName(Cols.ManterOMesmoEstadoCivil)]
    public bool? ManterOMesmoEstadoCivil { get; private set; }

    [JsonPropertyName(Cols.EstadoCivilAtual)]
    public int? EstadoCivil { get; private set; }

    [JsonPropertyName(Cols.DataDeCasamentoAtual)]
    public DateTimeOffset? DataDoCasamentoAtualOffset { get; private set; }

    [JsonIgnore]
    public DateOnly? DataDoCasamentoAtual
    {
        get => DateOnly.MinValue;
        set => DataDoCasamentoAtualOffset = DateTimeOffset.MinValue;
    }

    [JsonPropertyName(Cols.RegimeDeBensAtual)]
    public int? RegimeDeBens { get; private set; }

    [JsonPropertyName(Cols.SituacaoAtualDoAuxilioMoradia)]
    public int? SituacaoAtual { get; private set; }

    [JsonPropertyName($"{Cols.SituacaoAtualDoAuxilioMoradia}@x")]
    public string? SituacaoAtualFormatted { get; private set; }

    [JsonPropertyName(Cols.Nib)]
    public string? Nib { get; private set; }

    [JsonPropertyName(Cols.DataDeAlteracaoDaSituacao)]
    public DateOnly? DataDeAlteracaoDaSituacao { get; private set; }

    [JsonPropertyName(Cols.DataDoPactoAnteNupcialAtual)]
    public DateTimeOffset? DataDoPactoAntenupcialOffset { get; private set; }

    [JsonIgnore]
    public DateOnly? DataDoPactoAntenupcial
    {
        get => DateOnly.MinValue;
        set => DataDoPactoAntenupcialOffset = DateTimeOffset.MinValue;
    }

    [JsonPropertyName(Cols.PactoAntenupcialLivroAtual)]
    public string? Livro { get; private set; }

    [JsonPropertyName(Cols.PactoAntenupcialFolhaAtual)]
    public string? Folha { get; private set; }

    [JsonPropertyName(Cols.PactoAntenupcialDataAtual)]
    public DateTimeOffset? DataEstadoCivilOffset { get; private set; }

    [JsonIgnore]
    public DateOnly? DataEstadoCivil
    {
        get => DateOnly.MinValue;
        set => DataEstadoCivilOffset = DateTimeOffset.MinValue;
    }

    [JsonPropertyName(Cols.NomeDoConjugeAtual)]
    public string? NomeDoConjuge { get; private set; }

    [JsonPropertyName(Cols.EmailDoConjugeAtual)]
    public string? EmailDoConjuge { get; private set; }

    [JsonPropertyName(Cols.RgDoConjugeAtual)]
    public string? RgDoConjuge { get; private set; }

    [JsonPropertyName(Cols.CpfDoConjugeAtual)]
    public string? CpfDoConjuge { get; private set; }

    [JsonPropertyName(Cols.UfRgDoConjugeAtual)]
    public int? UfRgDoConjuge { get; private set; }

    [JsonPropertyName(Cols.DataDeNascimentoDoConjugeAtual)]
    public DateOnly? DataDeNascimentoDoConjuge { get; private set; }

    [JsonPropertyName(Cols.NacionalidadeDoConjugeAtual)]
    public string? NacionalidadeDoConjuge { get; private set; }

    [JsonPropertyName(Cols.ProfissaoDoConjugeAtual)]
    public int? ProfissaoDoConjuge { get; private set; }

    [JsonPropertyName(Cols.NomeDaMaeDoConjugeAtual)]
    public string? NomeDaMaeDoConjuge { get; private set; }

    [JsonPropertyName(Cols.CorRacaDoConjugeAtual)]
    public int? RacaCorDoConjuge { get; private set; }

    [JsonPropertyName(Cols.EstadoCivilDoConjugeAtual)]
    public int? EstadoCivilDoConjuge { get; private set; }

    [JsonPropertyName(Cols.GrauDeEscolaridade)]
    public int? GrauDeEscolaridade { get; private set; }

    [JsonPropertyName(Cols.GrauDeEscolaridadeDoConjugeAtual)]
    public int? GrauDeEscolaridadeDoConjuge { get; private set; }

    [JsonPropertyName(Cols.NomeDoCompanheiroDoConjuge)]
    public string? NomeDoCompanheiroDoConjuge { get; private set; }

    [JsonPropertyName(Cols.NomeSocial)]
    public string? NomeSocialDoConjuge { get; private set; }

    [JsonPropertyName(Cols.ConfirmacaoDaVeracidadeDasInformacoes)]
    public bool? ConfirmacaoDaVeracidadeDasInformacoes { get; private set; }

    [JsonPropertyName(Cols.ProfissaoDoTitular)]
    public int? Profissao { get; private set; }

    [JsonPropertyName(Cols.QuantasPessoasPossuemRenda)]
    public int? QuantasPessoasPossuemRenda { get; private set; }

    [JsonPropertyName(Cols.ResponsavelDoSeuNucleoFamiliarEhMulher)]
    public bool? MulherEhResponsavelPeloNucleoFamiliar { get; private set; }

    [JsonPropertyName(Cols.ValorDoAluguel)]
    public decimal? ValorMensalDoAluguel { get; private set; }

    [JsonPropertyName(Cols.AlguemDoNucleoFamiliarQueResideComVocePossuiDeficiencia)]
    public bool? AlguemDoNucleoFamiliarPossuiDeficiencia { get; private set; }

    [JsonPropertyName(Cols.ODeficienteEhVoceSeuConjugeOuCompanheroAscendentesDescendetesTuteladosOuCuratelados)]
    public bool? APessoaComDeficienciaEhVoce { get; private set; }

    [JsonPropertyName(Cols.HouveMudancaDeConjugeOuCompanheiroAposORecebimentoDoAuxilio)]
    public bool? HouveMudancaDeConjugeOuCompanheiroAposORecebimentoDoAuxilio { get; private set; }

    [JsonPropertyName(Cols.TipoDeDeficiencia)]
    public int? QualTipoDeDeficiencia { get; private set; }

    [JsonPropertyName(Cols.EhCadeirante)]
    public bool? VoceEhCadeirante { get; private set; }

    [JsonPropertyName(Cols.TemNecessidadeDeUmaUnidadeHabitacionalAdaptada)]
    public bool? NecessitaDeUnidadeHabitacionalAdaptada { get; private set; }

    [JsonPropertyName(Cols.SistemaDeOrigem)]
    public int? SistemaDeOrigem { get; private set; }

    [JsonPropertyName(Cols.SexoDoConjuge)]
    public int? SexoDoConjuge { get; private set; }

    [JsonPropertyName(Cols.RendaDoConjuge)]
    public decimal? RendaDoConjuge { get; private set; }

    [JsonPropertyName(Cols.RendaDoTitular)]
    public decimal? RendaAtual { get; private set; }

    [JsonIgnore]
    public int? UfDeNascimentoDoConjuge
    {
        get => 0;
        private set => UfDeNascimentoDoConjugeString = "";
    }

    [JsonPropertyName(Cols.UfDeNascimentoDoConjuge)]
    public string? UfDeNascimentoDoConjugeString { get; private set; }

    [JsonPropertyName(Cols.DossieId)]
    public Guid? DossieId { get; private set; }

    [JsonPropertyName(Cols.EmpreendimentoInscricaoId)]
    public Guid? EmpreendimentoInscricaoId { get; private set; }

    [JsonPropertyName(Cols.DataDeInicioDoAuxilioMoradia)]
    public DateTimeOffset? DataDeInicioDoAuxilioMoradiaOffSet { get; private set; }

    [JsonPropertyName(Cols.Pessoas)]
    public Pessoa[] Pessoas { get; private set; } = Array.Empty<Pessoa>();

    [JsonIgnore]
    public bool TemPessoa => true;

    [JsonIgnore]
    public Guid? PessoaId => Guid.NewGuid();

    [JsonIgnore]
    public DateOnly? DataDeInicioDoAuxilioMoradia
    {
        get => default;
        set => DataDeInicioDoAuxilioMoradiaOffSet = default;
    }

    [JsonPropertyName(Cols.RendaFamiliar)]
    public decimal? RendaFamiliar { get; private set; }

    [JsonPropertyName(Cols.ComponenteFamiliarConjugeAtualId)]
    public Guid? ComponenteFamiliarConjugeAtualId { get; private set; }

    [JsonPropertyName(Cols.ComponenteFamiliarCompanheiroAtualId)]
    public Guid? ComponenteFamiliarCompanheiroAtualId { get; private set; }

    [JsonPropertyName(Cols.ViveEmUniaoEstavel)]
    public bool? ViveEmUniaoEstavel { get; private set; }

    [JsonPropertyName(Cols.ConfirmaQueEhCasadoMasViveComOutraPessoa)]
    public bool? ConfirmaQueEhCasadoMasViveComOutraPessoa { get; private set; }

    [JsonPropertyName(Cols.Situacao)]
    public int? StateCode { get; private set; }

    public ClientePotencial(string nomeCompleto, string cpf)
    {
        NomeCompleto = nomeCompleto;
        Cpf = cpf;
        SistemaDeOrigem = 3;
    }

    public ClientePotencial(
        string nomeCompleto,
        string cpf,
        DateOnly dataDeNascimento,
        string email,
        Guid empreendimentoInscricaoId)
    {
        NomeCompleto = nomeCompleto;
        Cpf = cpf;
        DataDeNascimento = dataDeNascimento;
        Email = email;
        EmpreendimentoInscricaoId = empreendimentoInscricaoId;
    }

    [JsonIgnore]
    public TitularVo Titular => new TitularVo();
    [JsonIgnore]
    public TitularVo Moradia => new TitularVo();
    [JsonIgnore]
    public TitularVo SituacaoDoAuxilio => new TitularVo();
}


public class TitularVo
{

}