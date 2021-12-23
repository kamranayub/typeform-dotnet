using System.Linq;
using System.Text.Json;

namespace Typeform.Tests;

public class AnswerDeserializationTests
{
  private T GetAnswerFromFixture<T>(int index) where T : TypeformAnswer
  {
    var responsesFixture = GetResponsesFixture();
    var responses = JsonSerializer.Deserialize<TypeformResponsesContainer>(responsesFixture, TypeformClient.DefaultSystemTextJsonSerializerOptions);

    return responses!.Items[0].Answers.GetAnswer<T>(index);
  }

  [Fact]
  public void Deserializes_Answer_Text_Field()
  {
    var answer = GetAnswerFromFixture<TypeformTextAnswer>(0);

    Assert.Equal(AnswerType.Text, answer.Type);
    Assert.Equal("Job opportunities", answer.Text);
  }

  [Fact]
  public void Deserializes_Answer_Boolean_Field()
  {
    var answer = GetAnswerFromFixture<TypeformBooleanAnswer>(1);
    
    Assert.Equal(AnswerType.Boolean, answer.Type);
    Assert.False(answer.Boolean);
  }

  [Fact]
  public void Deserializes_Answer_Email_Field()
  {
    var answer = GetAnswerFromFixture<TypeformEmailAnswer>(4);
    
    Assert.Equal(AnswerType.Email, answer.Type);
    Assert.Equal("lian1078@other.com", answer.Email);
  }

  [Fact]
  public void Deserializes_Answer_Number_Field()
  {
    var answer = GetAnswerFromFixture<TypeformNumberAnswer>(5);
    
    Assert.Equal(AnswerType.Number, answer.Type);
    Assert.Equal(1, answer.Number);
  }

  [Fact]
  public void Falls_Back_When_Deserializing_Unknown_Answer_Field()
  {
    var answer = GetAnswerFromFixture<TypeformAnswer>(2);
    
    Assert.Equal(AnswerType.Unknown, answer.Type);
  }

  private string GetResponsesFixture()
  {
    return System.IO.File.ReadAllText("fixtures/responses.json");
  }
}