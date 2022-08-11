using System;

namespace Cblx.Blocks.Models;

internal sealed class HandlerDeclaration
{

	public HandlerDeclaration(string name, string? routePrefix = null)
	{
		Name = name;
		RoutePrefix = routePrefix;
	}

	public string Name { get; private set; }
	public string? RoutePrefix { get; private set; }


	public string BuildRoute()
	{
		throw new NotImplementedException();
	}
}
