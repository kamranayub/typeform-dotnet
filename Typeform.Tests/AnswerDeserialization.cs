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

    Assert.Equal(AnswerType.Text, responses!.Items[0].Answers[0].Type);
    var textAnswer = responses.Items[0].Answers.GetAnswer<TypeformTextAnswer>(0);
    Assert.NotNull(textAnswer);
    Assert.Equal("Job opportunities", textAnswer.Text);
  }

  [Fact]
  public void Deserializes_Answer_Email_Field()
  {
    var responsesFixture = GetResponsesFixture();
    var responses = JsonSerializer.Deserialize<TypeformResponsesContainer>(responsesFixture, TypeformClient.DefaultSystemTextJsonSerializerOptions);

    Assert.Equal(AnswerType.Email, responses!.Items[0].Answers[4].Type);
    var emailAnswer = responses.Items[0].Answers.GetAnswer<TypeformEmailAnswer>(4);
    Assert.NotNull(emailAnswer);
    Assert.Equal("lian1078@other.com", emailAnswer.Email);
  }

  [Fact]
  public void Deserializes_Answer_Boolean_Field()
  {
    var responsesFixture = GetResponsesFixture();
    var responses = JsonSerializer.Deserialize<TypeformResponsesContainer>(responsesFixture, TypeformClient.DefaultSystemTextJsonSerializerOptions);

    Assert.Equal(AnswerType.Boolean, responses!.Items[0].Answers[1].Type);
    var booleanAnswer = responses.Items[0].Answers.GetAnswer<TypeformBooleanAnswer>(1);
    Assert.NotNull(booleanAnswer);
    Assert.False(booleanAnswer.Boolean);
  }

  [Fact]
  public void Falls_Back_When_Deserializing_Unknown_Answer_Field()
  {
    var responsesFixture = GetResponsesFixture();
    var responses = JsonSerializer.Deserialize<TypeformResponsesContainer>(responsesFixture, TypeformClient.DefaultSystemTextJsonSerializerOptions);

    Assert.Equal(AnswerType.Unknown, responses!.Items[0].Answers[2].Type);
  }

  private string GetResponsesFixture()
  {
    return System.IO.File.ReadAllText("fixtures/responses.json");
  }
}