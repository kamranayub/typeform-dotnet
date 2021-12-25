using Typeform.Json.Serialization;
using System.Text.Json.Serialization;

namespace Typeform;

public class TypeformClient
{

  /// <summary>
  /// Default System.Text.Json JsonSerializerOptions that supports Typeform's API contract
  /// </summary>
  /// <returns></returns>
  public static JsonSerializerOptions DefaultSystemTextJsonSerializerOptions => new JsonSerializerOptions()
  {
    PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy(),
    Converters = {
        new TypeformAnswerJsonConverter(),
        new TypeformVariableJsonConverter(),
        new JsonStringEnumMemberConverter(
          new JsonStringEnumMemberConverterOptions() {
            DeserializationFailureFallbackValue = TypeformAnswerType.Unknown
          }, typeof(TypeformAnswerType)),
        new JsonStringEnumMemberConverter(
          new JsonStringEnumMemberConverterOptions() {
            DeserializationFailureFallbackValue = TypeformVariableType.Unknown
          }, typeof(TypeformVariableType)),
          new JsonStringEnumMemberConverter(
        new JsonStringEnumMemberConverterOptions() {
            DeserializationFailureFallbackValue = TypeformQuestionType.Unknown
          }, typeof(TypeformQuestionType))
        },
  };

  /// <summary>
  /// Sets up Refit ContentSerializer to support Typeform API contracts using System.Text.Json
  /// </summary>
  /// <value></value>
  public static RefitSettings DefaultSettings => new RefitSettings
  {
    CollectionFormat = CollectionFormat.Csv,
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

  private static async Task<(int readCount, byte[] buffer)> ReadChunkAsync(Stream stream, int chunkSize)
  {
    var buffer = new byte[chunkSize];
    var readCount = 0;

    while (readCount < chunkSize)
    {
      var bytesRead = await stream.ReadAsync(buffer, readCount, chunkSize - readCount);

      if (bytesRead == 0)
      {
        break;
      }

      readCount += bytesRead;
    }

    return (readCount, buffer);
  }

  /// <summary>
  /// TODO: Move?
  /// </summary>
  /// <param name="stream"></param>
  /// <returns></returns>
  public static async Task<byte[]> ReadChunkedStreamAsync(Stream stream)
  {
    IEnumerable<byte> contents = new byte[0];
    int lastRead;

    do
    {
      var (readCount, buffer) = await TypeformClient.ReadChunkAsync(stream, 4096);
      lastRead = readCount;
      contents = contents.Concat(buffer);
    } while (lastRead > 0);

    return contents.ToArray();
  }
}