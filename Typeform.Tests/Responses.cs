using System;
using System.Text.Json;

namespace Typeform.Tests;

public class ResponsesTests
{
  private TypeformResponse GetResponseFromFixture(int responseIndex)
  {
    var responsesFixture = FixturesHelper.GetResponsesFixture();
    var responses = JsonSerializer.Deserialize<TypeformResponsesContainer>(responsesFixture, TypeformClient.DefaultSystemTextJsonSerializerOptions);

    return responses!.Items[responseIndex];
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
  public void Deserializes_Response_Hidden()
  {
    var response = GetResponseFromFixture(0);
    Assert.Equal(true, response.Hidden.Value<bool>("bool"));
    Assert.Equal("abc", response.Hidden.Value<string>("string"));
    Assert.Equal(1, response.Hidden.Value<int>("number"));
  }

  [Fact]
  public void Deserializes_Response_Variables_Number()
  {
    var response = GetResponseFromFixture(0);
    Assert.Equal("score", response.Variables[0].Key);
    Assert.Equal("number", response.Variables[0].Type);
    Assert.Equal(2, response.Variables[0].Number);
  }

  [Fact]
  public void Deserializes_Response_Variables_Text()
  {
    var response = GetResponseFromFixture(0);
    Assert.Equal("name", response.Variables[0].Key);
    Assert.Equal("text", response.Variables[0].Type);
    Assert.Equal("typeform", response.Variables[0].Text);
  }
}