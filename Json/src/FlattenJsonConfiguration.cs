using System.Linq.Expressions;

namespace Cblx.Blocks;

public abstract class FlattenJsonConfiguration<T> : FlattenJsonConfiguration
     where T : class
{
    public FlattenJsonConfiguration<T> HasJsonPropertyName(Expression<Func<T, object>> member, string jsonPropertyName)
    {
        if (member.Body is MemberExpression memberExpression)
        {
            _propertyMappings[memberExpression.Member.Name] = jsonPropertyName;
        }
        else if (member.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression unaryMemberExpression)
        {
            _propertyMappings[unaryMemberExpression.Member.Name] = jsonPropertyName;
        }

        return this;
    }
}

public abstract class FlattenJsonConfiguration
{
    protected readonly Dictionary<string, string> _propertyMappings = new();

    public string GetJsonPropertyName(string propertyName)
    {
        if (_propertyMappings.TryGetValue(propertyName, out var jsonPropertyName))
        {
            return jsonPropertyName;
        }

        return propertyName;
    }
}