using System;
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
  public void Deserializes_Answer_Choices_Field()
  {
    var answer = GetAnswerFromFixture<TypeformChoicesAnswer>(10);

    Assert.Equal(AnswerType.Choices, answer.Type);
    Assert.NotNull(answer.Choices);
    Assert.NotEmpty(answer.Choices.Labels);
    Assert.Contains("New York", answer.Choices.Labels);
    Assert.Contains("Tokyo", answer.Choices.Labels);
  }

  [Fact]
  public void Deserializes_Answer_Date_Field()
  {
    var answer = GetAnswerFromFixture<TypeformDateAnswer>(11);

    Assert.Equal(AnswerType.Date, answer.Type);
    var expectedDate = new DateTime(2012, 3, 20, 0, 0, 0, DateTimeKind.Utc);
    Assert.Equal(expectedDate, answer.Date);
  }

  public void Deserializes_Answer_Choice_Field()
  {
    var answer = GetAnswerFromFixture<TypeformChoiceAnswer>(12);

    Assert.Equal(AnswerType.Choice, answer.Type);
    Assert.NotNull(answer.Choice);
    Assert.Equal("A friend's experience in Sydney", answer.Choice.Label);
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