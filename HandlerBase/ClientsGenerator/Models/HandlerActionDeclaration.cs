using Cblx.Blocks.Enums;

namespace Cblx.Blocks.Models;

internal sealed class HandlerActionDeclaration
{
	public HandlerActionDeclaration(
		string name, 
		HttpVerb verb, 
		HandlerReturnDeclaration returnDeclaration, 
		HandlerParameterDeclaration? parameterDeclaration)
	{
		Name = name;
		Verb = verb;
		ReturnDeclaration = returnDeclaration;
		ParameterDeclaration = parameterDeclaration;
	}

	public string Name { get; private set; }
	public HttpVerb Verb { get; private set; }

	public HandlerReturnDeclaration ReturnDeclaration { get; private set; }	
	public HandlerParameterDeclaration? ParameterDeclaration { get; private set; }
}
