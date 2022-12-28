namespace Cblx.Blocks.Models;

internal class HandlerParameterDeclaration
{
    public HandlerParameterDeclaration(string name, string typeName, string methodParameterFormat, string parameterNamespace)
    {
        Name = name;
        TypeName = typeName;
        MethodParameterFormat = methodParameterFormat;
        Namespace = parameterNamespace;
    }

    public string Name { get; }
    public string TypeName { get; }
    public string Namespace { get; }
    public string MethodParameterFormat { get; }
}