namespace Typeform.Models;

public class TypeformResponse
{
  public TypeformResponse()
  {
    Answers = new TypeformResponseAnswers();
    Variables = new TypeformResponseVariables();
    Hidden = new TypeformValueDictionary();
    Metadata = new TypeformResponseMetadata();
    Calculated = new TypeformResponseCalculatedFields();
  }

  /// <summary>
  /// Unique ID for the response. Note that `response_id` values are unique per form but are not unique globally.
  /// </summary>
  /// <value></value>
  public string ResponseId { get; set; }

  public string LandingId { get; set; }

  /// <summary>
  /// Time of the form landing. In ISO 8601 format, UTC time, to the second, with T as a delimiter between the date and time
  /// </summary>
  /// <value></value>
  public DateTime LandedAt { get; set; }

  /// <summary>
  /// Time that the form response was submitted. In ISO 8601 format, UTC time, to the second, with T as a delimiter between the date and time.
  /// </summary>
  /// <value></value>
  public DateTime SubmittedAt { get; set; }

  public string Token { get; set; }

  public TypeformResponseCalculatedFields Calculated { get; set; }

  public TypeformValueDictionary Hidden { get; set; }

  public TypeformResponseAnswers Answers { get; set; }

  public TypeformResponseVariables Variables { get; set; }

  /// <summary>
  /// Metadata about a client's HTTP request.
  /// </summary>
  /// <value></value>
  public TypeformResponseMetadata Metadata { get; set; }
}