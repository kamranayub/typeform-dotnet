using System;
using System.Text.Json;

namespace Typeform.Tests;

public class ResponsesTests
{
  [Fact]
  public void Deserializes_Response_Top_Level_Fields()
  {
    var responsesFixture = FixturesHelper.GetResponsesFixture();
    var responses = JsonSerializer.Deserialize<TypeformResponsesContainer>(responsesFixture, TypeformClient.DefaultSystemTextJsonSerializerOptions);

    var expectedLandedAt = new DateTime(2017, 9, 14, 22, 33, 59, DateTimeKind.Utc);
    Assert.Equal(expectedLandedAt, responses!.Items[0].LandedAt);
  }
}