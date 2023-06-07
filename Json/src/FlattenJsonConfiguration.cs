using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Cblx.Blocks;

public abstract class FlattenJsonConfiguration<T> : FlattenJsonConfiguration
     where T : class
{
    protected FlattenJsonConfiguration() : base(typeof(T))
    {
    }

    public FlattenJsonConfiguration<T> HasJsonPropertyName(Expression<Func<T, object?>> member, string jsonPropertyName)
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
    private readonly Type _type;
    public FlattenJsonConfiguration(Type type)
    {
        _type = type;
    }

    protected readonly Dictionary<string, string> _propertyMappings = new();

    public string GetJsonPropertyName(string propertyName)
    {
        if (_propertyMappings.TryGetValue(propertyName, out var jsonPropertyName))
        {
            return jsonPropertyName;
        }
        if(_type.GetProperty(propertyName) is PropertyInfo propertyInfo)
        {
            var jsonPropertyNameAttribute = propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>();
            if (jsonPropertyNameAttribute is not null)
            {
                return jsonPropertyNameAttribute.Name;
            }
        }
        return propertyName;
    }
}