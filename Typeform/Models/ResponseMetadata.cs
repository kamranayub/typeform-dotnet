namespace Typeform.Models;

public class TypeformResponseMetadata
{
  public string Browser { get; set; }

  /// <summary>
  /// IP of the client
  /// </summary>
  /// <value></value>
  public string NetworkId { get; set; }

  /// <summary>
  /// Derived from user agent
  /// </summary>
  /// <value></value>
  public string Platform { get; set; }

  public string Referer { get; set; }

  public string UserAgent { get; set; }
}
