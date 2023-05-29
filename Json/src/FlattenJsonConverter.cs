using System.Text.Json.Serialization;
using System.Text.Json;
using System.Reflection;

namespace Cblx.Blocks;

// Inicialmente gerado pela AI do Bing, modificado manualmente para suportar Fluent e construtor privado
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
        var properties = typeof(T).GetProperties().ToDictionary(p => p.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? p.Name);
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                return value;
            }

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();
                if (properties.TryGetValue(propertyName!, out PropertyInfo? property))
                {
                    var propertyValue = JsonSerializer.Deserialize(ref reader, property.PropertyType, options);
                    property.SetValue(value, propertyValue);
                }
                else
                {
                    var flattenProperty = properties.Values.FirstOrDefault(p => p.GetCustomAttribute<FlattenAttribute>() != null);
                    if (flattenProperty != null)
                    {
                        var flattenPropertyValue = flattenProperty.GetValue(value);
                        if (flattenPropertyValue == null)
                        {
                            flattenPropertyValue = options.GetTypeInfo(flattenProperty.PropertyType).CreateObject!();
                            flattenProperty.SetValue(value, flattenPropertyValue);
                        }
                        var nestedProperties = flattenProperty.PropertyType.GetProperties().ToDictionary(p => p.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? p.Name);

                        if (flattenProperty.GetCustomAttribute<FlattenAttribute>() is FlattenAttribute flattenAttributeGeneric && flattenAttributeGeneric.ConfigurationType != null)
                        {
                            var configuration = Activator.CreateInstance(flattenAttributeGeneric.ConfigurationType) as FlattenJsonConfiguration;
                            nestedProperties = nestedProperties.ToDictionary(kvp => configuration?.GetJsonPropertyName(kvp.Key) ?? kvp.Key, kvp => kvp.Value);
                        }

                        if (nestedProperties.TryGetValue(propertyName!, out PropertyInfo? nestedProperty))
                        {
                            var propertyValue = JsonSerializer.Deserialize(ref reader, nestedProperty.PropertyType, options);
                            nestedProperty.SetValue(flattenPropertyValue, propertyValue);
                        }
                    }
                }
            }
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            var propertyName = property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? property.Name;
            var flattenAttribute = property.GetCustomAttribute<FlattenAttribute>();
            if (flattenAttribute != null && property.PropertyType.IsClass)
            {
                var nestedProperties = property.PropertyType.GetProperties();
                foreach (var nestedProperty in nestedProperties)
                {
                    var nestedPropertyName = nestedProperty.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? nestedProperty.Name;

                    if (flattenAttribute is FlattenAttribute flattenAttributeGeneric && flattenAttributeGeneric.ConfigurationType != null)
                    {
                        var configuration = Activator.CreateInstance(flattenAttributeGeneric.ConfigurationType) as FlattenJsonConfiguration;
                        nestedPropertyName = configuration?.GetJsonPropertyName(nestedPropertyName) ?? nestedPropertyName;
                    }

                    var nestedValue = nestedProperty.GetValue(property.GetValue(value));
                    writer.WritePropertyName(nestedPropertyName);
                    JsonSerializer.Serialize(writer, nestedValue, options);
                }
            }
            else
            {
                var propertyValue = property.GetValue(value);
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, propertyValue, options);
            }
        }
        writer.WriteEndObject();
    }

}