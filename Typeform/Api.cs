namespace Typeform;

/// <summary>
/// The public Typeform API contract. Use with Refit to generate a client.
/// </summary>
public interface ITypeformApi
{
  /// <summary>
  /// Typeform Responses API
  /// </summary>
  /// <param name="accessToken">The OAuth or Personal Access Token to authorize the call</param>
  /// <param name="formId">Unique ID for the form. Find in your form URL. For example, in the URL "https://mysite.typeform.com/to/u6nXL7" the form_id is u6nXL7.</param>
  /// <returns></returns>
  [Get("/forms/{formId}/responses")]
  Task<TypeformResponseItems> GetFormResponsesAsync(
    [Authorize("Bearer")] string accessToken,
    string formId);

  /// <summary>
  /// Typeform Responses API
  /// </summary>
  /// <param name="accessToken">The OAuth or Personal Access Token to authorize the call</param>
  /// <param name="formId">Unique ID for the form. Find in your form URL. For example, in the URL "https://mysite.typeform.com/to/u6nXL7" the form_id is u6nXL7.</param>
  /// <param name="queryParams">Optional query parameters to pass to Responses endpoint</param>
  /// <returns></returns>
  [Get("/forms/{formId}/responses")]
  Task<TypeformResponseItems> GetFormResponsesAsync(
    [Authorize("Bearer")] string accessToken,
    string formId,
    TypeformGetResponsesParameters queryParams);

  /// <summary>
  /// Delete responses to a form.
  /// </summary>
  /// <param name="accessToken">The OAuth or Personal Access Token to authorize the call</param>
  /// <param name="formId">The form ID to access</param>
  /// <param name="includedResponseIds">List of response_id values of the responses to delete. You can list up to 1000 tokens.</param>
  [Delete("/forms/{formId}/responses")]
  Task<TypeformResponseItems> DeleteFormResponsesAsync(
    [Authorize("Bearer")] string accessToken,
    string formId,
    [AliasAs("included_response_ids")] string[] includedResponseIds);

  /// <summary>
  /// Retrieves a file stream from Typeform Form response. Use <see cref="TypeformStreamExtensions.ReadAllBytesAsync" /> to read all bytes
  /// at once.
  /// </summary>
  /// <param name="accessToken">The OAuth or Personal Access Token to authorize the call</param>
  /// <param name="formId">The form ID to access</param>
  /// <param name="responseId">The response ID to access</param>
  /// <param name="fieldId">The response field ID that holds the file upload</param>
  /// <param name="filename">The filename to access</param>
  /// <returns></returns>
  [Get("/forms/{formId}/responses/{responseId}/fields/{fieldId}/files/{filename}")]
  Task<ApiResponse<Stream>> GetFormResponseFileStreamAsync(
    [Authorize("Bearer")] string accessToken,
    string formId,
    string responseId,
    string fieldId,
    string filename);
}

public static class TypeformApiExtensions
{

  /// <summary>
  /// Attempts to retrieve an uploaded file from its URL provided by the Typeform API. *Officially*, this is "not supported"
  /// but in many circumstances, the <c>file_url</c> value returned by Typeform follows the convention of the REST API endpoint.
  /// </summary>
  /// <param name="api">The <see cref="ITypeformApi" /> reference</param>
  /// <param name="accessToken">The OAuth or Personal Access Token to authorize</param>
  /// <param name="fileUrl">A <c>file_url</c> in the format returned by Typeform Responses API</param>
  /// <returns></returns>
  public static Task<ApiResponse<Stream>> GetFormResponseFileStreamFromUrlAsync(
    this ITypeformApi api,
    string accessToken,
    Uri fileUri
  )
  {
    // Attempt to parse the file URL into its constituent parts
    var uriPath = fileUri.GetComponents(UriComponents.Path, UriFormat.Unescaped);
    var uriParts = uriPath.Split('/');

    if (uriParts.Length < 8)
    {
      throw new ArgumentException("File URI does not contain enough parts to match Typeform REST endpoint.", nameof(fileUri));
    }

    var formId = uriParts[1];
    var responseId = uriParts[3];
    var fieldId = uriParts[5];
    var filename = uriParts[7];

    return api.GetFormResponseFileStreamAsync(accessToken, formId, responseId, fieldId, filename);
  }
}

public static class TypeformStreamExtensions
{
  /// <summary>
  /// Reads the HTTP file stream using <see cref="System.IO.MemoryStream" />, returning all bytes.
  /// </summary>
  /// <param name="response">The Refit API response from Typeform containing a response stream</param>
  /// <returns></returns>
  public static async Task<byte[]> ReadAllBytesAsync(this ApiResponse<Stream> response)
  {
    using var memoryStream = new MemoryStream();
    await response.Content.CopyToAsync(memoryStream);
    return memoryStream.ToArray();
  }
}

public class TypeformGetResponsesParameters
{
  /// <summary>
  /// Maximum number of responses. Default value is 25. Maximum value is 1000. 
  /// If your typeform has fewer than 1000 responses, you can retrieve all of 
  /// the responses in a single request by adding the page_size parameter. 
  /// If your typeform has more than 1000 responses, use the since/until 
  /// or before/after query parameters to narrow the scope of your request.
  /// </summary>
  /// <value></value>
  public int? PageSize { get; set; }

  /// <summary>
  /// Limit request to responses submitted since the specified date and time. 
  /// Could be passed as int (timestamp in seconds) or in ISO 8601 format, UTC time, 
  /// to the second, with T as a delimiter between the date and time (2020-03-20T14:00:59).
  /// </summary>
  /// <value></value>
  public string Since { get; set; }

  /// <summary>
  /// Limit request to responses submitted until the specified date and time. 
  /// Could be passed as int (timestamp in seconds) or in ISO 8601 format, UTC time, 
  /// to the second, with T as a delimiter between the date and time (2020-03-20T14:00:59).
  /// </summary>
  /// <value></value>
  public string Until { get; set; }

  /// <summary>
  /// Limit request to responses submitted after the specified token. Could 
  /// not be used together with sort parameter, as it sorts responses in 
  /// the order that our system processed them (submitted_at). This ensures 
  /// that you can traverse the complete set of responses without repeating entries.
  /// </summary>
  /// <value></value>
  public string After { get; set; }

  /// <summary>
  /// Limit request to responses submitted before the specified token. Could 
  /// not be used together with sort parameter, as it sorts responses in the 
  /// order that our system processed them (submitted_at). This ensures that 
  /// you can traverse the complete set of responses without repeating entries.
  /// </summary>
  /// <value></value>
  public string Before { get; set; }

  /// <summary>
  /// Limit request to the specified response_id values. 
  /// Use a comma-separated list to specify more than one response_id value.
  /// </summary>
  /// <value></value>
  public string[] IncludedResponseIds { get; set; }

  /// <summary>
  /// Comma-separated list of response_ids to be excluded from the response.
  /// </summary>
  /// <value></value>
  public string[] ExcludedResponseIds { get; set; }

  /// <summary>
  /// Limit responses only to those which were submitted. This parameter 
  /// changes since/until filter, so if completed=true, it will filter by 
  /// submitted_at, otherwise - landed_at.
  /// </summary>
  /// <value></value>
  public bool Completed { get; set; }

  /// <summary>
  /// Responses order in {fieldID},{asc|desc} format. 
  /// You can use built-in submitted_at/landed_at field IDs or 
  /// any field ID from your typeform, possible directions are asc/desc. 
  /// Default value is submitted_at,desc.
  /// </summary>
  /// <value></value>
  public string Sort { get; set; }

  /// <summary>
  /// Limit request to only responses that include the specified string. 
  /// The string will be escaped and it will be matched aganist all answers 
  /// fields, hidden fields and variables values.
  /// </summary>
  /// <value></value>
  public string Query { get; set; }

  /// <summary>
  /// Show only specified fields in answers section. If response does not 
  /// have answers for specified fields, there will be null. Use a 
  /// comma-separated list to specify more than one field value.
  /// </summary>
  /// <value></value>
  public string[] Fields { get; set; }

  /// <summary>
  /// Limit request to only responses that include the specified fields in 
  /// answers section. Use a comma-separated list to specify more than 
  /// one field value - response will contain at least one of the specified fields.
  /// </summary>
  /// <value></value>
  public string[] AnsweredFields { get; set; }
}
