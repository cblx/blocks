using System.Threading.Tasks;
using Cblx.Blocks.Factories;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Cblx.Blocks.Helpers;

internal static class AnalyzerHelperExtensions
{
    public static void AnalyzeAndAddUsing(this ReturnDeclarationDto returnDeclaration, GeneratorExecutionContext context, SyntaxNode? node)
    {
        var usingResult = node switch
        {
            IdentifierNameSyntax syntax => CodeHelpers.GetNamespace(context, syntax) ?? "",
            GenericNameSyntax syntax => CodeHelpers.GetNamespace(context, syntax) ?? "",
            _ => string.Empty
        };

        if (string.IsNullOrWhiteSpace(usingResult)) return;
        
        returnDeclaration.Uses.Add(usingResult);
    }
    
    public static void AppendToken(this ReturnDeclarationDto returnDeclaration, SyntaxToken? syntaxToken)
    {
        if(syntaxToken.HasValue is false) return;

        var token = syntaxToken.Value.ToFullString().Trim();

        if (token is nameof(Task) or nameof(ValueTask))
        {
            returnDeclaration.HasAsync = true;
            returnDeclaration.HasVoid = true;
        }
        else
        {
            returnDeclaration.HasVoid = false;
        }

        returnDeclaration.ManipulationTypeStringBuilder.Append(token);
    }
    
    public static string ManipulationTypeBuilder(this ReturnDeclarationDto returnDeclaration)
    {
        if (returnDeclaration is { HasAsync: true, HasVoid: true }) return returnDeclaration.ManipulationTypeStringBuilder.ToString();
        
        returnDeclaration.ManipulationTypeStringBuilder.Replace($"{nameof(ValueTask)}<", string.Empty);
        returnDeclaration.ManipulationTypeStringBuilder.Replace($"{nameof(Task)}<", string.Empty);
        returnDeclaration.ManipulationTypeStringBuilder.Remove(returnDeclaration.ManipulationTypeStringBuilder.Length - 1, 1);
        return returnDeclaration.ManipulationTypeStringBuilder.ToString();
    }

}
