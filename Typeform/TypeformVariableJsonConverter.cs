
using System.Text.Json.Serialization;

namespace Typeform;

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

      TypeformVariableType type = TypeformVariableType.Unknown;
      string rawJson = jsonDocument.RootElement.GetRawText();
      var modifiedOptions = new JsonSerializerOptions(options);
      var existingConverter = modifiedOptions.Converters.FirstOrDefault(c => c is TypeformVariableJsonConverter);
      if (existingConverter != null)
      {
        modifiedOptions.Converters.Remove(existingConverter);
      }

      // TODO: This could be optimized maybe to avoid deserializing entire object?
      // { type: "text" }
      var typePropertyJson = $"{{ \"type\": \"{typeProperty.GetString()}\" }}";
      var defaultVariable = JsonSerializer.Deserialize<TypeformVariable>(typePropertyJson, modifiedOptions);

      type = defaultVariable.Type;

      Type variableInstanceType;

      switch (type)
      {
        case TypeformVariableType.Text:
          variableInstanceType = typeof(TypeformVariableText);
          break;
        case TypeformVariableType.Number:
          variableInstanceType = typeof(TypeformVariableNumber);
          break;
        default:
          return defaultVariable;
      }

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