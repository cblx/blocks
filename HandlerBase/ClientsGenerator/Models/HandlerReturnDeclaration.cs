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

    public string TypeName { get; private set; }
    public string MethodReturnFormat { get; private set; }
    public string ManipulationFormat { get; private set; }

    public bool HasVoid { get; private set; }
    public bool HasAsync { get; private set; }
}
