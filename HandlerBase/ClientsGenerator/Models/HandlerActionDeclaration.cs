namespace Cblx.Blocks.Models;

internal sealed class HandlerActionDeclaration
{
	public HandlerActionDeclaration(string name)
	{
		Name = name;
	}

    public string Name { get; private set; }
}
