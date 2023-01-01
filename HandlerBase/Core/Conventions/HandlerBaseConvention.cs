using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Cblx.Blocks.Conventions;
public class HandlerBaseConvention : IApplicationModelConvention
{
    public void Apply(ApplicationModel application)
    {
        foreach (var action in application.Controllers
            .Where(c => typeof(HandlerBase).IsAssignableFrom(c.ControllerType))
            .SelectMany(ac => ac.Actions))
        {
            var verb = GetHttpMethodVerb(action.ActionName);
            ConfigureActionConstraintByHttpVerb(action, verb);
            ConfigureBidingSourceByHttpVerb(action, verb);
        }
    }

    private static string? GetHttpMethodVerb(string actionName)
        => actionName switch
        {
            _ when actionName.StartsWith("Post") => "POST",
            _ when actionName.StartsWith("Get") => "GET",
            _ when actionName.StartsWith("Delete") => "DELETE",
            _ => default
        };

    private static void ConfigureActionConstraintByHttpVerb(ActionModel? action, string? verb)
    {
        if (verb is null) return;
        action?.Selectors.First().ActionConstraints.Add(new HttpMethodActionConstraint(new[] { verb }));
    }

    private static void ConfigureBidingSourceByHttpVerb(ActionModel? action, string? verb)
    {
        if (action is null || !action.Parameters.Any()) return;

        action.Parameters.First().BindingInfo = verb switch
        {
            "DELETE" or "GET" => new BindingInfo { BindingSource = BindingSource.Query },
            _ => action.Parameters.First().BindingInfo
        };
    }
}