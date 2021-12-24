using System;
using System.Text.Json;

namespace Typeform.Tests;

public class ResponsesTests
{
  private TypeformResponse GetResponseFromFixture(int responseIndex)
  {
    var responsesFixture = FixturesHelper.GetResponsesFixture();
    var responses = JsonSerializer.Deserialize<TypeformResponseItems>(responsesFixture, TypeformClient.DefaultSystemTextJsonSerializerOptions);

    return responses!.Items[responseIndex];
  }

  [Fact]
  public void Deserializes_Response_Answer_By_Index()
  {
    var response = GetResponseFromFixture(0);
    var answerByIndex = response.Answers.GetAnswer<TypeformAnswerText>(0);
    Assert.NotNull(answerByIndex);
    Assert.Equal("Job opportunities", answerByIndex.Text);
  }

  [Fact]
  public void Deserializes_Response_Answer_By_Field_Id()
  {
    var response = GetResponseFromFixture(0);
    var answerById = response.Answers.GetAnswerById<TypeformAnswerText>("hVONkQcnSNRj");
    Assert.NotNull(answerById);
    Assert.Equal("Job opportunities", answerById.Text);
  }

  [Fact]
  public void Deserializes_Response_Answer_By_Field_Ref()
  {
    var response = GetResponseFromFixture(0);
    var answerById = response.Answers.GetAnswerByRef<TypeformAnswerText>("my_custom_dropdown_reference");
    Assert.NotNull(answerById);
    Assert.Equal("Job opportunities", answerById.Text);
  }

  [Fact]
  public void Deserializes_Response_LandedAt()
  {
    var response = GetResponseFromFixture(0);
    var expectedLandedAt = new DateTime(2017, 9, 14, 22, 33, 59, DateTimeKind.Utc);
    Assert.Equal(expectedLandedAt, response.LandedAt);
  }

  [Fact]
  public void Deserializes_Response_LandingId()
  {
    var response = GetResponseFromFixture(0);
    Assert.Equal("21085286190ffad1248d17c4135ee56f", response.LandingId);
  }

  [Fact]
  public void Deserializes_Response_ResponseId()
  {
    var response = GetResponseFromFixture(0);
    Assert.Equal("21085286190ffad1248d17c4135ee56f", response.ResponseId);
  }

  [Fact]
  public void Deserializes_Response_Token()
  {
    var response = GetResponseFromFixture(0);
    Assert.Equal("test21085286190ffad1248d17c4135ee56f", response.Token);
  }

  [Fact]
  public void Deserializes_Response_Calculated()
  {
    var response = GetResponseFromFixture(0);
    Assert.Equal(2, response.Calculated.Score);
  }

  [Fact]
  public void Deserializes_Response_Hidden()
  {
    var response = GetResponseFromFixture(0);
    Assert.True(response.Hidden.Value<bool>("bool"));
    Assert.Equal("abc", response.Hidden.Value<string>("string"));
    Assert.Equal(1, response.Hidden.Value<int>("number"));
  }

  [Fact]
  public void Deserializes_Response_Variables_Number()
  {
    var response = GetResponseFromFixture(0);
    var variable = response.Variables.GetVariable<TypeformVariableNumber>("score");
    Assert.Equal("score", variable.Key);
    Assert.Equal(TypeformVariableType.Number, variable.Type);
    Assert.Equal(2, variable.Number);
  }

  [Fact]
  public void Deserializes_Response_Variables_Text()
  {
    var response = GetResponseFromFixture(0);
    var variable = response.Variables.GetVariable<TypeformVariableText>("name");

    Assert.Equal("name", variable.Key);
    Assert.Equal(TypeformVariableType.Text, variable.Type);
    Assert.Equal("typeform", variable.Text);
  }

  [Fact]
  public void Deserializes_Response_Metadata()
  {
    var response = GetResponseFromFixture(0);
    var expectedMetadata = new TypeformResponseMetadata()
    {
      Browser = "default",
      NetworkId = "responsdent_network_id",
      Platform = "other",
      Referer = "https://user_id.typeform.com/to/lR6F4j",
      UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_6) AppleWebKit/603.3.8 (KHTML, like Gecko) Version/10.1.2 Safari/603.3.8"
    };

    Assert.Equal(expectedMetadata.Browser, response.Metadata.Browser);
    Assert.Equal(expectedMetadata.NetworkId, response.Metadata.NetworkId);
    Assert.Equal(expectedMetadata.Platform, response.Metadata.Platform);
    Assert.Equal(expectedMetadata.Referer, response.Metadata.Referer);
    Assert.Equal(expectedMetadata.UserAgent, response.Metadata.UserAgent);
  }
}