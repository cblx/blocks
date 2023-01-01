using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace Cblx.Blocks;

public class HandlerControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    private readonly TypeInfo[] _types;

    public HandlerControllerFeatureProvider(TypeInfo[] types)
    {
        _types = types;
    }

    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        foreach(var t in _types)
        {
            feature.Controllers.Add(t);
        }
    }
}