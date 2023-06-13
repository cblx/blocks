using System.Text.Json.Serialization;
using System.Text.Json;
using System.Reflection;

namespace Cblx.Blocks;

public class FlattenJsonConverter<T> : JsonConverter<T>
{
    private readonly FlattenJsonConfiguration _rootConfiguration;

    public FlattenJsonConverter()
    {
        var rootConfigurationType = typeof(T).GetCustomAttribute<FlattenJsonRootAttribute>()?.ConfigurationType;
        _rootConfiguration = rootConfigurationType is null ? 
                new FlattenJsonConfiguration(typeof(T)) : 
                (Activator.CreateInstance(rootConfigurationType) as FlattenJsonConfiguration)!;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        // Esse converter também tem que fornecer a habilidade de desserialização
        // usando construtores não públicos.
        // A solução utilizando o TypeInfoResolver.Modifiers não vale aqui,
        // pois ao adicionar qualquer Converter customizado a lib muda o Kind para .None
        // e desabilita a possibilidade de setarmos o .CreateObject no JsonTypeInfo.
        // Isso deve ocorrer também com as propriedades com setters privados.
        var value = (T)Activator.CreateInstance(typeof(T), nonPublic: true)!;
        ReadProperties(ref reader, value, options, _rootConfiguration);
        return value;
    }

    private static void ReadProperties(ref Utf8JsonReader reader,
                                       object value,
                                       JsonSerializerOptions options,
                                       FlattenJsonConfiguration configuration)
    {
        Dictionary<PropertyData, object> nestedInstances = new();
        object FindOrCreateNestedInstances(PropertyData data)
        {
            if(data.ParentData is null)
            {
                return value;
            }
            if(nestedInstances.ContainsKey(data))
            {
                return nestedInstances[data];
            }
            var nestedInstance = data.ParentData.PropertyInfo.GetValue(FindOrCreateNestedInstances(data.ParentData))!;
            if(nestedInstance is null)
            {
                nestedInstance = Activator.CreateInstance(data.ParentData.PropertyInfo.PropertyType)!;
                data.ParentData.PropertyInfo.SetValue(FindOrCreateNestedInstances(data.ParentData), nestedInstance);
            }
            nestedInstances.Add(data, nestedInstance);
            return nestedInstance;
        }
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString()!;
                reader.Read();

                var data = configuration.FindDataByJsonPropertyName(propertyName);
                if (data is null || !configuration.ShouldInclude(data.PropertyInfo))
                {
                    reader.TrySkip(); // Skip the property value if it doesn't match any property in the dictionary
                    continue;
                }
                var propertyInfo = data.PropertyInfo;
                var propertyValue = JsonSerializer.Deserialize(ref reader, propertyInfo.PropertyType, options);
                var instance = FindOrCreateNestedInstances(data);
                propertyInfo.DeclaringType!.GetProperty(propertyInfo.Name)!.SetValue(instance, propertyValue);
            }
        }
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        WriteProperties(writer, value, options, _rootConfiguration);
        writer.WriteEndObject();
    }

    private static void WriteProperties(
        Utf8JsonWriter writer,
        object value,
        JsonSerializerOptions options,
        FlattenJsonConfiguration configuration
        )
    {
        var propertiesData = configuration.GetIncludedPropertiesData();
        var nestedInstances = new Dictionary<PropertyData, object>();
        object FindNestedInstance(PropertyData data)
        {
            if (data.ParentData is null)
            {
                return value;
            }
            if (nestedInstances.ContainsKey(data))
            {
                return nestedInstances[data];
            }
            var nestedInstance = data.ParentData.PropertyInfo.GetValue(FindNestedInstance(data.ParentData))!;
            nestedInstances.Add(data, nestedInstance);
            return nestedInstance;
        }

        foreach (var data in propertiesData)
        {
            if (data.IsFlatten) { continue; }
            var propertyName = configuration.GetJsonPropertyName(data.PropertyInfo);
            var instance = FindNestedInstance(data);
            var propertyValue = data.PropertyInfo.GetValue(instance);
            if (propertyValue != null)
            {
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, propertyValue, options);
            }
            else
            {
                writer.WriteNull(propertyName);
            }
        }
    }
}