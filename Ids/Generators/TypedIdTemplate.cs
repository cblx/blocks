namespace Cblx.Blocks.Ids.Generators;

internal static class TypedIdTemplate
{
    public static string Create(string name, string namespaceName)
    {
        return $$"""
                 // Auto-generated code
                 #nullable enable
                 
                 using System;
                 using System.ComponentModel;
                 using System.Diagnostics.CodeAnalysis;
                 using System.Text.Json.Serialization;
                 using Cblx.Blocks;
                 
                 namespace {{namespaceName}};
                 
                 [ExcludeFromCodeCoverage]
                 [TypeConverter(typeof(IdTypeConverter<{{name}}>))]
                 [JsonConverter(typeof(IdConverterFactory<{{name}}>))]
                 public readonly partial record struct {{name}}(Guid Guid)
                 {
                     public {{name}}(string guidString) : this(new Guid(guidString)){}
                     
                     public static implicit operator Guid({{name}}? id) => id?.Guid ?? Guid.Empty;
                     public static implicit operator Guid?({{name}}? id) => id?.Guid;
                     public static explicit operator {{name}}(Guid guid) => new(guid);
                     
                     public static {{name}} Empty { get; } = new(Guid.Empty);
                     public static {{name}} NewId() => new(Guid.NewGuid());
                     
                     public override string ToString() => Guid.ToString();
                 }
                 """;
    }
}