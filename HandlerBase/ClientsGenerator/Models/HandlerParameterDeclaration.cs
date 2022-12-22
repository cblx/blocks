namespace Cblx.Blocks.Models;

internal class HandlerParameterDeclaration
{
    public HandlerParameterDeclaration(string name, string typeName, string methodParameterFormat)
    {
        Name = name;
        TypeName = typeName;
        MethodParameterFormat = methodParameterFormat;
    }

    public string Name { get; }
    public string TypeName { get; }
    public string MethodParameterFormat { get; }
}