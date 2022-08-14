using System;

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

	public string InterfaceName { get; private set; }
	public string ImplementationName { get; private set; }
    public string HandlerNamespace { get; private set; }
	public string? RoutePrefix { get; private set; }


	public HandlerActionDeclaration HandlerAction { get; private set; }

	public string CreateAsyncToken()
	{
		return HandlerAction.ReturnDeclaration.HasAsync ? " async " : "";
	}
	

}
