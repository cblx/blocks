namespace Cblx.Blocks.Models;

internal sealed class HandlerDeclaration
{

	public HandlerDeclaration(
		string name, 
		string handlerNamespace, 
		HandlerActionDeclaration handlerAction, 
		string? routePrefix = null)
	{
		InterfaceName = name;
		ImplementationName = name.Substring(1);
		RoutePrefix = routePrefix;
		HandlerNamespace = handlerNamespace;
		HandlerAction = handlerAction;
	}

	public string InterfaceName { get; }
	public string ImplementationName { get; }
    public string HandlerNamespace { get; }
	public string? RoutePrefix { get; }


	public HandlerActionDeclaration HandlerAction { get; }

	public string CreateAsyncToken()
	{
		return HandlerAction.ReturnDeclaration.HasAsync ? "async" : "";
	}
	

}
