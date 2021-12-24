using System;
using System.Linq;
using System.Text.Json;

namespace Typeform.Tests;

public class AnswerDeserializationTests
{
  private T GetAnswerFromFixture<T>(int answerIndex, int itemIndex = 0) where T : TypeformAnswer
  {
    var responsesFixture = FixturesHelper.GetResponsesFixture();
    var responses = JsonSerializer.Deserialize<TypeformResponseItems>(responsesFixture, TypeformClient.DefaultSystemTextJsonSerializerOptions);

    return responses!.Items[itemIndex].Answers.GetAnswer<T>(answerIndex);
  }

  [Fact]
  public void Deserializes_Answer_Text_Field()
  {
    var answer = GetAnswerFromFixture<TypeformAnswerText>(0);

    Assert.Equal(TypeformAnswerType.Text, answer.Type);
    Assert.Equal("Job opportunities", answer.Text);
  }

  [Fact]
  public void Deserializes_Answer_Boolean_Field()
  {
    var answer = GetAnswerFromFixture<TypeformAnswerBoolean>(1);

    Assert.Equal(TypeformAnswerType.Boolean, answer.Type);
    Assert.False(answer.Boolean);
  }

  [Fact]
  public void Deserializes_Answer_Email_Field()
  {
    var answer = GetAnswerFromFixture<TypeformAnswerEmail>(4);

    Assert.Equal(TypeformAnswerType.Email, answer.Type);
    Assert.Equal("lian1078@other.com", answer.Email);
  }

  [Fact]
  public void Deserializes_Answer_Number_Field()
  {
    var answer = GetAnswerFromFixture<TypeformAnswerNumber>(5);

    Assert.Equal(TypeformAnswerType.Number, answer.Type);
    Assert.Equal(1, answer.Number);
  }

  [Fact]
  public void Deserializes_Answer_Choices_Field()
  {
    var answer = GetAnswerFromFixture<TypeformAnswerChoices>(10);

    Assert.Equal(TypeformAnswerType.Choices, answer.Type);
    Assert.NotNull(answer.Choices);
    Assert.NotEmpty(answer.Choices.Labels);
    Assert.Contains("New York", answer.Choices.Labels);
    Assert.Contains("Tokyo", answer.Choices.Labels);
  }

  [Fact]
  public void Deserializes_Answer_Date_Field()
  {
    var answer = GetAnswerFromFixture<TypeformAnswerDate>(11);

    Assert.Equal(TypeformAnswerType.Date, answer.Type);
    var expectedDate = new DateTime(2012, 3, 20, 0, 0, 0, DateTimeKind.Utc);
    Assert.Equal(expectedDate, answer.Date);
  }

  [Fact]
  public void Deserializes_Answer_Choice_Field()
  {
    var answer = GetAnswerFromFixture<TypeformAnswerChoice>(12);

    Assert.Equal(TypeformAnswerType.Choice, answer.Type);
    Assert.NotNull(answer.Choice);
    Assert.Equal("A friend's experience in Sydney", answer.Choice.Label);
  }

  [Fact]
  public void Deserializes_Answer_FileUrl_Field()
  {
    var answer = GetAnswerFromFixture<TypeformAnswerFileUrl>(1, itemIndex: 1);
    var expectedUri = new Uri("https://api.typeform.com/forms/lT9Z2j/responses/7f46165474d11ee5836777d85df2cdab/fields/X4BgU2f1K6tG/files/afd8258fd453-aerial_view_rural_city_latvia_valmiera_urban_district_48132860.jpg");

    Assert.Equal(TypeformAnswerType.FileUrl, answer.Type);
    Assert.Equal(expectedUri, answer.FileUrl);
  }

  [Fact]
  public void Deserializes_Answer_Url_Field()
  {
    var answer = GetAnswerFromFixture<TypeformAnswerUrl>(15);
    var expectedUri = new Uri("https://www.google.com");

    Assert.Equal(TypeformAnswerType.Url, answer.Type);
    Assert.Equal(expectedUri, answer.Url);
  }

  [Fact]
  public void Deserializes_Answer_Payment_Field()
  {
    var answer = GetAnswerFromFixture<TypeformAnswerPayment>(16);
    var expectedPaymentData = new TypeformAnswerPaymentData()
    {
      Amount = "$1.00",
      Name = "Franz Tester",
      Last4 = "1234"
    };

    Assert.Equal(TypeformAnswerType.Payment, answer.Type);
    Assert.Equal(expectedPaymentData.Amount, answer.Payment.Amount);
    Assert.Equal(expectedPaymentData.Name, answer.Payment.Name);
    Assert.Equal(expectedPaymentData.Last4, answer.Payment.Last4);
  }

  [Fact]
  public void Falls_Back_When_Deserializing_Unknown_Answer_Field()
  {
    var answer = GetAnswerFromFixture<TypeformAnswer>(2);

    Assert.Equal(TypeformAnswerType.Unknown, answer.Type);
  }
}