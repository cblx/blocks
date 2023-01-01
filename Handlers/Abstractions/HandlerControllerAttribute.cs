namespace Cblx.Blocks;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class HandlerControllerAttribute : Attribute
{
	//  TODO: Pensar se precisamos suportar rotas customizadas, ex : /api/oque-eu-quiser
	//public string? Template { get; private set; }

	//public HandlerControllerAttribute(string? template = null)
	//{
	//       Template = template;
	//}
}