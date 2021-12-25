using System.Collections.Generic;
using System.Linq;
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

  [Fact]
  public async Task Client_Can_Download_File_From_Responses_Api()
  {
    var accessToken = Configuration["TypeformAccessToken"];

    // TODO: This file_url is available in the response
    // and it would be nice to figure out how to pass
    // that in and get the downloaded file
    var fileStream = await _api.GetFormResponseFile(
      accessToken,
      "Mj5yRSHu",
      "8kvscilox7xp42d58c8kvsc39l6ct2rg",
      "a6MMDcSis1p1",
      "c001c8c70d77-derwinternaht.zip"
    );

    var contents = await TypeformClient.ReadChunkedStreamAsync(fileStream);
    Assert.NotNull(contents);
    Assert.Equal(1_699_840, contents.Length);

    // write to file system
    // await System.IO.File.WriteAllBytesAsync("test.zip", contents);
  }
}