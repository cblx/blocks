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

	public string Name { get; }
	public HttpVerb Verb { get; }

	public HandlerReturnDeclaration ReturnDeclaration { get; }	
	public HandlerParameterDeclaration? ParameterDeclaration { get; }
}
