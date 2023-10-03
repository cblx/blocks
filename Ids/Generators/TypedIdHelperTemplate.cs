namespace Cblx.Blocks.Ids.Generators;

public  static class TypedIdHelperTemplate
{
    public static string CreateTypeOfLine(string namespaceBase, string name)
    {
        return $"typeof({namespaceBase}.{name}),";
    }
    
    public static string Create(string namespaceName, string ids)
    {
        return $$"""
                 // Auto-generated code
                 #nullable enable

                 using System;
                 using System.Diagnostics.CodeAnalysis;

                 namespace {{namespaceName}};

                 [ExcludeFromCodeCoverage]
                 public static class TypedIdHelper
                 {
                     public static Type[] Types { get; } = 
                     {
                         {{ids}}
                     }; 
                 }
                 """;
    }
}