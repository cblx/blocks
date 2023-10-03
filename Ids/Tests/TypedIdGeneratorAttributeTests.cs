using System.Text;
using Cblx.Blocks.Ids.Generators;
using Cblx.Blocks.SourceGenerators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using VerifyCS =
    Cblx.Blocks.SourceGenerators.CSharpSourceGeneratorVerifier<Cblx.Blocks.Ids.Generators.TypedIdGenerator>;

namespace Cblx.Blocks.Ids.Tests;

public class TypedIdGeneratorAttributeTests
{
    [Fact]
    public async Task DeveGerarIdTipadoCorretamente()
    {
        const string code = """
                            using Cblx.Blocks;
                            namespace MyNamespace;
                            [GenerateTypedId]
                            public class MyClass {}
                            """;

        const string generated = """
                                 // Auto-generated code
                                 #nullable enable

                                 using System;
                                 using System.ComponentModel;
                                 using System.Diagnostics.CodeAnalysis;
                                 using System.Text.Json.Serialization;
                                 using Cblx.Blocks;

                                 namespace MyNamespace;

                                 [ExcludeFromCodeCoverage]
                                 [TypeConverter(typeof(TypedIdTypeConverter<MyClassId>))]
                                 [JsonConverter(typeof(TypedIdConverterFactory<MyClassId>))]
                                 public readonly partial record struct MyClassId(Guid Guid)
                                 {
                                     public MyClassId(string guidString) : this(new Guid(guidString)){}
                                     
                                     public static implicit operator Guid(MyClassId? id) => id?.Guid ?? Guid.Empty;
                                     public static implicit operator Guid?(MyClassId? id) => id?.Guid;
                                     public static explicit operator MyClassId(Guid guid) => new(guid);
                                     
                                     public static MyClassId Empty { get; } = new(Guid.Empty);
                                     public static MyClassId NewId() => new(Guid.NewGuid());
                                     
                                     public override string ToString() => Guid.ToString();
                                 }
                                 """;

        const string helper = $$"""
                                // Auto-generated code
                                #nullable enable
                                
                                using System;
                                using System.Diagnostics.CodeAnalysis;
                                
                                namespace Cblx.Blocks;
                                
                                [ExcludeFromCodeCoverage]
                                public static class TypedIdHelper
                                {
                                    public static Type[] Types { get; } = 
                                    {
                                        typeof(MyNamespace.MyClassId),
                                
                                    }; 
                                }
                                """;

        await new VerifyCS.Test
        {
            TestState =
            {
                Sources = { code },
                GeneratedSources =
                {
                    (typeof(TypedIdGenerator), "MyClassId.g.cs", SourceText.From(generated, Encoding.UTF8)),
                    (typeof(TypedIdGenerator), "TypedIdHelper.g.cs", SourceText.From(helper, Encoding.UTF8)),
                },
                ReferenceAssemblies = Net.Net70,
                AdditionalReferences =
                {
                    MetadataReference.CreateFromFile(typeof(GenerateTypedIdAttribute).Assembly.Location)
                }
            },
        }.RunAsync();
    }
}