using System.Text.Json.Serialization;

namespace Typeform.Serialization;

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
        TypeformAnswerType.Boolean => typeof(TypeformBooleanAnswer),
        TypeformAnswerType.Choice => typeof(TypeformChoiceAnswer),
        TypeformAnswerType.Choices => typeof(TypeformChoicesAnswer),
        TypeformAnswerType.Date => typeof(TypeformDateAnswer),
        TypeformAnswerType.Email => typeof(TypeformEmailAnswer),
        TypeformAnswerType.FileUrl => typeof(TypeformFileUrlAnswer),
        TypeformAnswerType.Number => typeof(TypeformNumberAnswer),
        TypeformAnswerType.Payment => typeof(TypeformPaymentAnswer),
        TypeformAnswerType.Text => typeof(TypeformTextAnswer),
        TypeformAnswerType.Url => typeof(TypeformUrlAnswer),
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