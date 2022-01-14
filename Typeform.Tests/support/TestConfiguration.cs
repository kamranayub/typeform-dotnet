using Microsoft.Extensions.Configuration;

namespace Typeform.Tests;

public static class TestConfigurationBuilder
{
  public static TestConfiguration Build()
  {
    var builder = new ConfigurationBuilder()
        .AddUserSecrets<TestConfiguration>();
    var secrets = builder.Build();

    return new TestConfiguration(secrets);
  }
}

public class TestConfiguration
{
  internal TestConfiguration(IConfiguration configuration) {
    TypeformAccessToken = configuration["TypeformAccessToken"];
  }

  public string TypeformAccessToken { get; set; }
}