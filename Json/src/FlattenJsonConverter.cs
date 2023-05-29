using System.Text.Json.Serialization;
using System.Text.Json;
using System.Reflection;

namespace Cblx.Blocks;

// Inicialmente gerado pela AI do Bing, modificado manualmente para suportar Fluent e construtor privado, 
// e correções e suporte a melhor aninhamento utilizando o ChatGPT
public class FlattenJsonConverter<T> : JsonConverter<T>
{
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
        ReadProperties(ref reader, value, options);
        return value;
    }
    
    private void ReadProperties(ref Utf8JsonReader reader, object value, JsonSerializerOptions options)
    {
        var properties = value.GetType().GetProperties().ToDictionary(p => p.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? p.Name);
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();
                PropertyInfo property;
                if (properties.TryGetValue(propertyName, out property))
                {
                    var propertyValue = JsonSerializer.Deserialize(ref reader, property.PropertyType, options);
                    property.SetValue(value, propertyValue);
                }
                else
                {
                    var flattenProperties = properties.Values.Where(p => p.GetCustomAttribute<FlattenAttribute>() != null).ToList();
                    foreach (var flattenProperty in flattenProperties)
                    {
                        var flattenPropertyValue = flattenProperty.GetValue(value);
                        if (flattenPropertyValue == null)
                        {
                            flattenPropertyValue = options.GetTypeInfo(flattenProperty.PropertyType).CreateObject();
                            flattenProperty.SetValue(value, flattenPropertyValue);
                        }
                        var nestedProperties = flattenProperty.PropertyType.GetProperties().ToDictionary(p => p.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? p.Name);

                        if (flattenProperty.GetCustomAttribute<FlattenAttribute>() is FlattenAttribute flattenAttributeGeneric && flattenAttributeGeneric.ConfigurationType != null)
                        {
                            var configuration = Activator.CreateInstance(flattenAttributeGeneric.ConfigurationType) as FlattenJsonConfiguration;
                            nestedProperties = nestedProperties.ToDictionary(kvp => configuration?.GetJsonPropertyName(kvp.Key) ?? kvp.Key, kvp => kvp.Value);
                        }

                        PropertyInfo nestedProperty;
                        if (nestedProperties.TryGetValue(propertyName, out nestedProperty))
                        {
                            var propertyValue = JsonSerializer.Deserialize(ref reader, nestedProperty.PropertyType, options);
                            nestedProperty.SetValue(flattenPropertyValue, propertyValue);
                            ReadProperties(ref reader, flattenPropertyValue, options); // Chamar ReadProperties novamente para processar as propriedades aninhadas
                            break;
                        }
                        else
                        {
                            ReadProperties(ref reader, flattenPropertyValue, options);
                        }
                    }
                }
            }
        }
    }
   
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        WriteProperties(writer, value, options);
        writer.WriteEndObject();
    }

    private void WriteProperties(Utf8JsonWriter writer, object value, JsonSerializerOptions options, FlattenAttribute flattenAttribute = null)
    {
        var properties = value.GetType().GetProperties();
        foreach (var property in properties)
        {
            var propertyName = property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? property.Name;
            var currentFlattenAttribute = property.GetCustomAttribute<FlattenAttribute>();
            if (currentFlattenAttribute != null && property.PropertyType.IsClass)
            {
                var nestedValue = property.GetValue(value);
                if (nestedValue != null)
                {
                    WriteProperties(writer, nestedValue, options, currentFlattenAttribute);
                }
            }
            else
            {
                if (flattenAttribute is FlattenAttribute flattenAttributeGeneric && flattenAttributeGeneric.ConfigurationType != null)
                {
                    var configuration = Activator.CreateInstance(flattenAttributeGeneric.ConfigurationType) as FlattenJsonConfiguration;
                    propertyName = configuration?.GetJsonPropertyName(propertyName) ?? propertyName;
                }

                var propertyValue = property.GetValue(value);
                if (propertyValue != null)
                {
                    writer.WritePropertyName(propertyName);
                    JsonSerializer.Serialize(writer, propertyValue, options);
                }
            }
        }
    }
}