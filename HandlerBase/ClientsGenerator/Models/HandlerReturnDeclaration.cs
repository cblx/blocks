namespace Cblx.Blocks.Models;

internal class HandlerReturnDeclaration
{
    public HandlerReturnDeclaration(
        string typeName,
        string methodReturnFormat,
        string manipulationFormat,
        bool hasVoid,
        bool hasAsync,
        string returnNamespace)
    {
        TypeName = typeName;
        MethodReturnFormat = methodReturnFormat;
        ManipulationFormat = manipulationFormat;
        HasVoid = hasVoid;
        HasAsync = hasAsync;
        Namespace = returnNamespace;
    }

    public string TypeName { get; }
    public string MethodReturnFormat { get; }
    public string ManipulationFormat { get; }
    public string Namespace { get; }

    public bool HasVoid { get; }
    public bool HasAsync { get; }
}