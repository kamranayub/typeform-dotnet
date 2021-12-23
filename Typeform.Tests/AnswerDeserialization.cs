using System.Linq;
using System.Text.Json;

namespace Typeform.Tests;

public class AnswerDeserializationTests {

  [Fact]
  public void Deserializes_Answer_Text_Field() {
    var responsesFixture = GetResponsesFixture();
    var responses = JsonSerializer.Deserialize<TypeformResponsesContainer>(responsesFixture, TypeformClient.DefaultSystemTextJsonSerializerOptions);

    Assert.NotNull(responses);
    Assert.Equal(responses!.Items[0].Answers[0].Type, "text");
    Assert.Equal(responses.Items[0].Answers.Get<TypeformTextAnswer>(0).Text, "text");
  }

  private string GetResponsesFixture() {
    return System.IO.File.ReadAllText("fixtures/responses.json");
  }
}