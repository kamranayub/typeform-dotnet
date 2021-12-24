
using System.Text.Json.Serialization;

namespace Typeform.Serialization;

public class TypeformVariableJsonConverter : JsonConverter<TypeformVariable>
{

  public override bool CanConvert(Type typeToConvert) =>
          typeof(TypeformVariable).IsAssignableFrom(typeToConvert);

  public override TypeformVariable Read(
      ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    if (reader.TokenType != JsonTokenType.StartObject)
    {
      throw new JsonException();
    }

    using (var jsonDocument = JsonDocument.ParseValue(ref reader))
    {
      var typePropertyName = options.PropertyNamingPolicy.ConvertName(nameof(TypeformVariable.Type));
      if (!jsonDocument.RootElement.TryGetProperty(typePropertyName, out var typeProperty))
      {
        throw new JsonException();
      }

      var modifiedOptions = new JsonSerializerOptions(options);
      var existingConverter = modifiedOptions.Converters.FirstOrDefault(c => c is TypeformVariableJsonConverter);
      if (existingConverter != null)
      {
        modifiedOptions.Converters.Remove(existingConverter);
      }

      var typePropertyJson = $"{{ \"type\": \"{typeProperty.GetString()}\" }}";
      var defaultVariable = JsonSerializer.Deserialize<TypeformVariable>(typePropertyJson, modifiedOptions);

      TypeformVariableType type = defaultVariable.Type;
      Type variableInstanceType = type switch
      {
        TypeformVariableType.Text => typeof(TypeformVariableText),
        TypeformVariableType.Number => typeof(TypeformVariableNumber),
        _ => typeof(TypeformVariable)
      };

      var rawJson = jsonDocument.RootElement.GetRawText();
      var result = (TypeformVariable)JsonSerializer.Deserialize(
        rawJson, variableInstanceType, modifiedOptions);

      return result;
    }
  }

  public override void Write(
      Utf8JsonWriter writer, TypeformVariable variable, JsonSerializerOptions options)
  {
    JsonSerializer.Serialize(writer, (object)variable, options);
  }
}