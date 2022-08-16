namespace Cblx.Blocks.Models;

internal class HandlerReturnDeclaration
{
    public HandlerReturnDeclaration(
        string typeName, 
        string methodReturnFormat, 
        string manipulationFormat, 
        bool hasVoid, 
        bool hasAsync)
    {
        TypeName = typeName;
        MethodReturnFormat = methodReturnFormat;
        ManipulationFormat = manipulationFormat;
        HasVoid = hasVoid;
        HasAsync = hasAsync;
    }

    public string TypeName { get; }
    public string MethodReturnFormat { get; }
    public string ManipulationFormat { get; }

    public bool HasVoid { get; }
    public bool HasAsync { get; }
}
