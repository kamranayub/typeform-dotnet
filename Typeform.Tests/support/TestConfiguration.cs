using Microsoft.Extensions.Configuration;

namespace Typeform.Tests;

public class TestConfiguration
{
  public static TestConfiguration Build()
  {
    var builder = new ConfigurationBuilder()
        .AddUserSecrets<TestConfiguration>();
    var secrets = builder.Build();

    return new TestConfiguration(secrets);
  }

  private TestConfiguration(IConfiguration configuration) {
    TypeformAccessToken = configuration["TypeformAccessToken"];
  }

  public string TypeformAccessToken { get; }
}