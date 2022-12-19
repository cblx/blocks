namespace Cblx.Blocks.Templates;

internal static class InvalidOperationExceptionTemplate
{
    public static string Create(string message)
    {
        return $"""
                throw new InvalidOperationException("{message}");
        """;
    }
}