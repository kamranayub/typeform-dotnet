using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Typeform
{

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

        AnswerType type = AnswerType.Unknown;
        string rawJson = jsonDocument.RootElement.GetRawText();
        var modifiedOptions = new JsonSerializerOptions(options);
        var existingConverter = modifiedOptions.Converters.FirstOrDefault(c => c is TypeformAnswerJsonConverter);
        if (existingConverter != null)
        {
          modifiedOptions.Converters.Remove(existingConverter);
        }

        // TODO: This could be optimized maybe to avoid deserializing entire object?
        // { type: "text" }
        var typePropertyJson = $"{{ \"type\": \"{typeProperty.GetString()}\" }}";
        var defaultAnswer = JsonSerializer.Deserialize<TypeformAnswer>(typePropertyJson, modifiedOptions);

        type = defaultAnswer.Type;

        Type answerInstanceType;

        switch (type)
        {
          case AnswerType.Text:
            answerInstanceType = typeof(TypeformTextAnswer);
            break;
          case AnswerType.Boolean:
            answerInstanceType = typeof(TypeformBooleanAnswer);
            break;
          case AnswerType.Email:
            answerInstanceType = typeof(TypeformEmailAnswer);
            break;
          case AnswerType.Number:
            answerInstanceType = typeof(TypeformNumberAnswer);
            break;
          case AnswerType.Choices:
            answerInstanceType = typeof(TypeformChoicesAnswer);
            break;
          default:
            return defaultAnswer;
        }

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
}