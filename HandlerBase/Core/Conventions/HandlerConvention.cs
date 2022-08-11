using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace Cblx.Blocks.Conventions;
public class HandlerConvention : IApplicationModelConvention
{
    public void Apply(ApplicationModel application)
    {
        foreach (var action in application.Controllers.SelectMany(ac => ac.Actions))
        {
            string? verb = GetHttpMethodVerb(action.ActionName);
            ConfigureActionConstraintByHttpVerb(action, verb);
            ConfigureBidingSourceByHttpVerb(action, verb);
        }
    }

    private static string? GetHttpMethodVerb(string actionName)
        => actionName switch
        {
            var x when x.StartsWith("Post") => "POST",
            var x when x.StartsWith("Get") => "GET",
            var x when x.StartsWith("Delete") => "DELETE",
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