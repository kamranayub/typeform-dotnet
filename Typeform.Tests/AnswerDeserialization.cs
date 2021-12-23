using System.Linq;
using System.Text.Json;

namespace Typeform.Tests;

public class AnswerDeserializationTests
{

  [Fact]
  public void Deserializes_Answer_Text_Field()
  {
    var responsesFixture = GetResponsesFixture();
    var responses = JsonSerializer.Deserialize<TypeformResponsesContainer>(responsesFixture, TypeformClient.DefaultSystemTextJsonSerializerOptions);

    Assert.NotNull(responses);
    Assert.NotNull(responses!.Items);
    Assert.NotNull(responses.Items[0]);
    Assert.Equal(AnswerType.Text, responses.Items[0].Answers[0].Type);
    var textAnswer = responses.Items[0].Answers.Get<TypeformTextAnswer>(0);
    Assert.NotNull(textAnswer);
    Assert.Equal("Job opportunities", textAnswer.Text);
  }

  private string GetResponsesFixture()
  {
    return System.IO.File.ReadAllText("fixtures/responses.json");
  }
}