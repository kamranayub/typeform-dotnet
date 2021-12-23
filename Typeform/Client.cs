using System.Text.Json;
using Refit;

namespace Typeform
{

  public class TypeformClient
  {
  public static JsonSerializerOptions DefaultSystemTextJsonSerializerOptions => new JsonSerializerOptions()
  {
    PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy()
  };
    /// <summary>
    /// Note: Doesn't make sense for consumers to override naming policy!
    /// </summary>
    /// <value></value>
    public static RefitSettings DefaultSettings => new RefitSettings
    {
      ContentSerializer = new SystemTextJsonContentSerializer(DefaultSystemTextJsonSerializerOptions)
    };

    public static ITypeformApi CreateApi()
    {
      return CreateApi(DefaultSettings);
    }

    public static ITypeformApi CreateApi(RefitSettings settings)
    {
      return CreateApi("https://api.typeform.com", settings);
    }

    public static ITypeformApi CreateApi(string baseUrl)
    {
      return CreateApi(baseUrl, DefaultSettings);
    }

    public static ITypeformApi CreateApi(string baseUrl, RefitSettings settings)
    {
      // TODO: Ensure naming policy is set correctly

      var typeformApi = RestService.For<ITypeformApi>(
        baseUrl,
        settings
      );

      return typeformApi;
    }
  }
}