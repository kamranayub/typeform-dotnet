namespace Typeform.Models;

public class TypeformResponseItems
{

  public TypeformResponseItems()
  {
    Items = new List<TypeformResponse>();
  }

  /// <summary>
  /// Total number of items in the retrieved collection.
  /// </summary>
  /// <value></value>
  public int TotalItems { get; set; }

  /// <summary>
  /// Number of pages.
  /// </summary>
  /// <value></value>
  public int PageCount { get; set; }

  /// <summary>
  /// Array of Typeform Response objects
  /// </summary>
  /// <value></value>
  public IList<TypeformResponse> Items { get; set; }
}