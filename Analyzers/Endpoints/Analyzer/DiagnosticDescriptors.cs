using Microsoft.CodeAnalysis;

namespace Cblx.Blocks.Analyzers.Endpoints;

public static class DiagnosticDescriptors
{
    private const string CategoryDesign = "Design";
    
    public const string DiagnosticEndpointShouldContainExecuteAsyncMethodId = "CBLX0001";
    public const string DiagnosticExecuteAsyncMethodShouldBeStaticId = "CBLX0002";
    public const string DiagnosticExecuteAsyncMethodShouldBeInternalId = "CBLX0003";
    
    public static readonly DiagnosticDescriptor EndpointShouldContainExecuteAsyncMethod = new(
        id: DiagnosticEndpointShouldContainExecuteAsyncMethodId,
        title: CreateLocalizableString(nameof(Resources.CBLX0001Title)),
        messageFormat: CreateLocalizableString(nameof(Resources.CBLX0001MessageFormat)),
        category: CategoryDesign,
        description: CreateLocalizableString(nameof(Resources.CBLX0001Description)),
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);
    
    public static readonly DiagnosticDescriptor ExecuteAsyncMethodShouldBeStatic = new(
        id: DiagnosticExecuteAsyncMethodShouldBeStaticId,
        title: CreateLocalizableString(nameof(Resources.CBLX0002Title)),
        messageFormat: CreateLocalizableString(nameof(Resources.CBLX0002MessageFormat)),
        category: CategoryDesign,
        description: CreateLocalizableString(nameof(Resources.CBLX0002Description)),
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);
    
    public static readonly DiagnosticDescriptor ExecuteAsyncMethodShouldBeInternal = new(
        id: DiagnosticExecuteAsyncMethodShouldBeInternalId,
        title: CreateLocalizableString(nameof(Resources.CBLX0003Title)),
        messageFormat: CreateLocalizableString(nameof(Resources.CBLX0003MessageFormat)),
        category: CategoryDesign,
        description: CreateLocalizableString(nameof(Resources.CBLX0003Description)),
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);
    
    private static LocalizableResourceString CreateLocalizableString(string name) => new(name, Resources.ResourceManager, typeof(Resources));
}