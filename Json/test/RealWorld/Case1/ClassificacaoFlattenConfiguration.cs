namespace Cblx.Blocks.Json.Tests.RealWorld.Case1;

public class ClassificacaoFlattenConfiguration : FlattenJsonConfiguration<Classificacao>
{
    public ClassificacaoFlattenConfiguration()
    {
        HasJsonPropertyName(c => c.AreaId, TbAtendimento.Cols.AreaId);
        HasJsonPropertyName(c => c.AssuntoId, TbAtendimento.Cols.AssuntoId);
        HasJsonPropertyName(c => c.GrupoDeProblemasId, TbAtendimento.Cols.GrupoDeProblemaId);
        HasJsonPropertyName(c => c.ProblemaId, TbAtendimento.Cols.ProblemaId);
    }
}