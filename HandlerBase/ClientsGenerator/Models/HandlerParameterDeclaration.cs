namespace Cblx.Blocks.Models;

internal class HandlerParameterDeclaration
{
    public HandlerParameterDeclaration(string name, string typeName, string methodParameterFormat)
    {
        Name = name;
        TypeName = typeName;
        MethodParameterFormat = methodParameterFormat;
    }

    public string Name { get; private set; }
    public string TypeName { get; private set; }
    public string MethodParameterFormat { get; private set; }
}
