namespace Cblx.Blocks.Ids.Generators;

internal class SyntaxReceiver : ISyntaxReceiver
{
    public List<ClassDeclarationSyntax> Classes { get; } = new();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not ClassDeclarationSyntax classDeclarationSyntax) return;
        var attrListText = classDeclarationSyntax.AttributeLists.ToString();
        
        if (attrListText.Contains("GenerateTypedId"))
        {
            Classes.Add(classDeclarationSyntax);
        }
    }
}