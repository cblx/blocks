using Cblx.Blocks.Enums;

namespace Cblx.Blocks.Models;

internal sealed class HandlerActionDeclaration
{
	public HandlerActionDeclaration(string name, HttpVerb verb)
	{
		Name = name;
		Verb = verb;
	}

    public string Name { get; private set; }
	public HttpVerb Verb { get; private set; }
}
