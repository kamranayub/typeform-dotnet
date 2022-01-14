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
  public async Task Client_FormResponses_Api_Supports_Query_Parameter() {
var accessToken = Configuration["TypeformAccessToken"];
    var responses = await _api.GetFormResponsesAsync(
      accessToken,
      "xVMHX23n",
      new TypeformGetResponsesParameters() {
        Query = "SirRainJack"
      });

    Assert.Equal(1, responses.TotalItems);
  }

  [Fact]
  public async Task Client_Can_Download_File_From_Responses_Api()
  {
    var accessToken = Configuration["TypeformAccessToken"];

    // TODO: This file_url is available in the response
    // and it would be nice to figure out how to pass
    // that in and get the downloaded file
    var fileResponse = await _api.GetFormResponseFileStreamAsync(
      accessToken,
      "Mj5yRSHu",
      "8kvscilox7xp42d58c8kvsc39l6ct2rg",
      "a6MMDcSis1p1",
      "c001c8c70d77-derwinternaht.zip"
    );

    var contents = await fileResponse.ReadAllBytesAsync();
    Assert.NotNull(contents);
    Assert.Equal(1_692_347, contents.Length);

    // write to file system
    // await System.IO.File.WriteAllBytesAsync("test.zip", contents);
  }

  [Fact]
  public async Task Client_Can_Download_File_Using_File_Url_From_Responses_Api()
  {
    var accessToken = Configuration["TypeformAccessToken"];

    // TODO: This file_url is available in the response
    // and it would be nice to figure out how to pass
    // that in and get the downloaded file
    var fileResponse = await _api.GetFormResponseFileStreamFromUrlAsync(
      accessToken,
      new System.Uri("https://api.typeform.com/forms/Mj5yRSHu/responses/8kvscilox7xp42d58c8kvsc39l6ct2rg/fields/a6MMDcSis1p1/files/c001c8c70d77-derwinternaht.zip")
    );

    var contents = await fileResponse.ReadAllBytesAsync();
    Assert.NotNull(contents);
    Assert.Equal(1_692_347, contents.Length);

    // write to file system
    // await System.IO.File.WriteAllBytesAsync("test.zip", contents);
  }
}