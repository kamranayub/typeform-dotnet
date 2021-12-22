using Microsoft.Extensions.Configuration;

namespace Typeform.Tests;

public class IntegrationTests
{
  IConfiguration Configuration { get; set; }

  private ITypeformApi _api;

  public IntegrationTests()
  {
    // the type specified here is just so the secrets library can 
    // find the UserSecretId we added in the csproj file
    var builder = new ConfigurationBuilder()
        .AddUserSecrets<IntegrationTests>();

    Configuration = builder.Build();

    _api = TypeformClient.CreateApi();
  }

  // TODO: Categorize as integration test
  // TODO: Don't run in CI
  [Fact]
  public async Task Client_Receives_A_200_Success_From_Responses_Api()
  {
    var accessToken = Configuration["TypeformAccessToken"];
    var responses = await _api.GetFormResponsesAsync(
      accessToken,
      "xVMHX23n");

    Assert.True(responses.TotalItems > 0);
  }
}