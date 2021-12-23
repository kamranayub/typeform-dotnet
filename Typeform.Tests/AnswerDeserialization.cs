using System;
using System.Linq;
using System.Text.Json;

namespace Typeform.Tests;

public class AnswerDeserializationTests
{
  private T GetAnswerFromFixture<T>(int answerIndex, int itemIndex = 0) where T : TypeformAnswer
  {
    var responsesFixture = GetResponsesFixture();
    var responses = JsonSerializer.Deserialize<TypeformResponsesContainer>(responsesFixture, TypeformClient.DefaultSystemTextJsonSerializerOptions);

    return responses!.Items[itemIndex].Answers.GetAnswer<T>(answerIndex);
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

  public void Deserializes_Answer_FileUrl_Field()
  {
    var answer = GetAnswerFromFixture<TypeformFileUrlAnswer>(1, itemIndex: 1);
    var expectedUri = new Uri("https://api.typeform.com/forms/lT9Z2j/responses/7f46165474d11ee5836777d85df2cdab/fields/X4BgU2f1K6tG/files/afd8258fd453-aerial_view_rural_city_latvia_valmiera_urban_district_48132860.jpg");

    Assert.Equal(AnswerType.FileUrl, answer.Type);
    Assert.Equal(expectedUri, answer.FileUrl);
  }

  public void Deserializes_Answer_Url_Field()
  {
    var answer = GetAnswerFromFixture<TypeformUrlAnswer>(15);
    var expectedUri = new Uri("https://www.google.com");

    Assert.Equal(AnswerType.Url, answer.Type);
    Assert.Equal(expectedUri, answer.Url);
  }

  public void Deserializes_Answer_Payment_Field()
  {
    var answer = GetAnswerFromFixture<TypeformPaymentAnswer>(16);

    Assert.Equal(AnswerType.Payment, answer.Type);
    Assert.Equal(new TypeformPaymentAnswerData()
    {
      Amount = "$1.00",
      Name = "Franz Tester",
      Last4 = "1234"
    }, answer.Payment);
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