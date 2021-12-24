using System.Text.Json.Serialization;

namespace Typeform.Json.Serialization;

public class TypeformAnswerJsonConverter : JsonConverter<TypeformAnswer>
{

  public override bool CanConvert(Type typeToConvert) =>
          typeof(TypeformAnswer).IsAssignableFrom(typeToConvert);

  public override TypeformAnswer Read(
      ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    if (reader.TokenType != JsonTokenType.StartObject)
    {
      throw new JsonException();
    }

    using (var jsonDocument = JsonDocument.ParseValue(ref reader))
    {
      var typePropertyName = options.PropertyNamingPolicy.ConvertName(nameof(TypeformAnswer.Type));
      if (!jsonDocument.RootElement.TryGetProperty(typePropertyName, out var typeProperty))
      {
        throw new JsonException();
      }

      var modifiedOptions = new JsonSerializerOptions(options);
      var existingConverter = modifiedOptions.Converters.FirstOrDefault(c => c is TypeformAnswerJsonConverter);
      if (existingConverter != null)
      {
        modifiedOptions.Converters.Remove(existingConverter);
      }

      var typePropertyJson = $"{{ \"type\": \"{typeProperty.GetString()}\" }}";
      var defaultAnswer = JsonSerializer.Deserialize<TypeformAnswer>(typePropertyJson, modifiedOptions);

      TypeformAnswerType type = defaultAnswer.Type;
      Type answerInstanceType = type switch
      {
        TypeformAnswerType.Boolean => typeof(TypeformAnswerBoolean),
        TypeformAnswerType.Choice => typeof(TypeformAnswerChoice),
        TypeformAnswerType.Choices => typeof(TypeformAnswerChoices),
        TypeformAnswerType.Date => typeof(TypeformAnswerDate),
        TypeformAnswerType.Email => typeof(TypeformAnswerEmail),
        TypeformAnswerType.FileUrl => typeof(TypeformAnswerFileUrl),
        TypeformAnswerType.Number => typeof(TypeformAnswerNumber),
        TypeformAnswerType.Payment => typeof(TypeformAnswerPayment),
        TypeformAnswerType.Text => typeof(TypeformAnswerText),
        TypeformAnswerType.Url => typeof(TypeformAnswerUrl),
        _ => typeof(TypeformAnswer)
      };

      var rawJson = jsonDocument.RootElement.GetRawText();
      var result = (TypeformAnswer)JsonSerializer.Deserialize(
        rawJson, answerInstanceType, modifiedOptions);

      return result;
    }
  }

  public override void Write(
      Utf8JsonWriter writer, TypeformAnswer answer, JsonSerializerOptions options)
  {
    JsonSerializer.Serialize(writer, (object)answer, options);
  }
}