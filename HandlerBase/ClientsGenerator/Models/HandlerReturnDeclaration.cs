using System.Collections.Generic;

namespace Cblx.Blocks.Models;

internal class HandlerReturnDeclaration
{
    public HandlerReturnDeclaration(
        string methodReturnType,
        string manipulationType,
        bool hasVoid,
        bool hasAsync, 
        List<string> uses)
    {
        MethodReturnType = methodReturnType;
        ManipulationType = manipulationType;
        HasVoid = hasVoid;
        HasAsync = hasAsync;
        Uses = uses;
    }

    public string MethodReturnType { get; }
    public string ManipulationType { get; }
    public List<string> Uses { get; }

    public bool HasVoid { get; }
    public bool HasAsync { get; }
}