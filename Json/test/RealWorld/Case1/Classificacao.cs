namespace Cblx.Blocks.Json.Tests.RealWorld.Case1;
public record Classificacao(
    Guid? AreaId,
    Guid? AssuntoId,
    Guid? GrupoDeProblemasId,
    Guid? ProblemaId
)
{
    private Classificacao() : this(null, null, null, null) { }
    public static Classificacao Empty() => new();
};
