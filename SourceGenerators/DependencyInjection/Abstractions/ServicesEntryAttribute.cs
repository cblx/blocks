namespace Cblx.Blocks;

/// <summary>
/// Cria um método de extenção no IServiceCollection chamado .AddAllServices.
/// Todos os assemblies que possuírem o Prefixo informado devem
/// ter a referência ao Generator, pois os métodos gerados lá
/// serão utilizados no Assembly atual sem checagem alguma sobre suas existências.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public class ServicesEntryAttribute : Attribute
{
    public string Prefix { get; private set; }
    public ServicesEntryAttribute(string prefix)
    {
        Prefix = prefix;
    }
}