using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Typeform.Tests;

public class IntegrationTests
{
  public IntegrationTests()
  {
    Configuration = TestConfigurationBuilder.Build();
    Api = TypeformClient.CreateApi();
  }

  TestConfiguration Configuration { get; }
  ITypeformApi Api { get; }

  [Fact]
  public async Task Client_FormResponses_Api_Should_Return_Submitted_Responses()
  {
    var responses = await Api.GetFormResponsesAsync(
      Configuration.TypeformAccessToken,
      "xVMHX23n");

    Assert.Equal(36, responses.TotalItems);
  }

  [Fact]
  public async Task Client_FormResponses_Api_Should_Filter_By_Query_Data()
  {
    var responses = await Api.GetFormResponsesAsync(
      Configuration.TypeformAccessToken,
      "xVMHX23n",
      new TypeformGetResponsesParameters()
      {
        Query = "SirRainJack"
      });

    Assert.Equal(1, responses.TotalItems);
  }

  [Fact]
  public async Task Client_FormResponses_Api_Should_Return_Unsubmitted_Responses()
  {
    var responses = await Api.GetFormResponsesAsync(
      Configuration.TypeformAccessToken,
      "xVMHX23n",
      new TypeformGetResponsesParameters()
      {
        Completed = false
      });

    Assert.Equal(40, responses.TotalItems);
  }

  [Fact]
  public async Task Client_Can_Download_File_From_Responses_Api()
  {
    var fileResponse = await Api.GetFormResponseFileStreamAsync(
      Configuration.TypeformAccessToken,
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
    var exampleUriRawFormResponsesApi = new System.Uri("https://api.typeform.com/forms/Mj5yRSHu/responses/8kvscilox7xp42d58c8kvsc39l6ct2rg/fields/a6MMDcSis1p1/files/c001c8c70d77-derwinternaht.zip");

    var fileResponse = await Api.GetFormResponseFileStreamFromUrlAsync(
      Configuration.TypeformAccessToken,
      exampleUriRawFormResponsesApi
    );

    var contents = await fileResponse.ReadAllBytesAsync();
    Assert.NotNull(contents);
    Assert.Equal(1_692_347, contents.Length);

    // write to file system
    // await System.IO.File.WriteAllBytesAsync("test.zip", contents);
  }
}