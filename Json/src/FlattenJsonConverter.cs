using System.Text.Json.Serialization;
using System.Text.Json;
using System.Reflection;

namespace Cblx.Blocks;

// 100% gerado pela AI do Bing
public class FlattenJsonConverter<T> : JsonConverter<T>
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        var value = Activator.CreateInstance<T>();
        var properties = typeof(T).GetProperties().ToDictionary(p => p.Name);
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
                PropertyInfo property;
                if (properties.TryGetValue(propertyName, out property))
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
                            flattenPropertyValue = Activator.CreateInstance(flattenProperty.PropertyType);
                            flattenProperty.SetValue(value, flattenPropertyValue);
                        }
                        var nestedProperties = flattenProperty.PropertyType.GetProperties().ToDictionary(p => p.Name);
                        PropertyInfo nestedProperty;
                        if (nestedProperties.TryGetValue(propertyName, out nestedProperty))
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
            var flattenAttribute = property.GetCustomAttribute<FlattenAttribute>();
            if (flattenAttribute != null && property.PropertyType.IsClass)
            {
                var nestedProperties = property.PropertyType.GetProperties();
                foreach (var nestedProperty in nestedProperties)
                {
                    var nestedValue = nestedProperty.GetValue(property.GetValue(value));
                    writer.WritePropertyName(nestedProperty.Name);
                    JsonSerializer.Serialize(writer, nestedValue, options);
                }
            }
            else
            {
                var propertyValue = property.GetValue(value);
                writer.WritePropertyName(property.Name);
                JsonSerializer.Serialize(writer, propertyValue, options);
            }
        }
        writer.WriteEndObject();
    }
}