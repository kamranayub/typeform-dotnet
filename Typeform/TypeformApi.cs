using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Refit;

namespace Typeform
{

  public interface ITypeformApi
  {
    /// <summary>
    /// Typeform Responses API
    /// </summary>
    /// <param name="formId">Unique ID for the form. Find in your form URL. For example, in the URL "https://mysite.typeform.com/to/u6nXL7" the form_id is u6nXL7.</param>
    /// <returns></returns>
    [Get("/forms/{form_id}/responses")]
    Task<TypeformResponsesContainer> GetFormResponsesAsync(
      [Authorize("Bearer")] string accessToken,
      [AliasAs("form_id")] string formId);

    /// <summary>
    /// Typeform Responses API
    /// </summary>
    /// <param name="formId">Unique ID for the form. Find in your form URL. For example, in the URL "https://mysite.typeform.com/to/u6nXL7" the form_id is u6nXL7.</param>
    /// <param name="queryParams">Optional query parameters to pass to Responses endpoint</param>
    /// <returns></returns>
    [Get("/forms/{form_id}/responses")]
    Task<TypeformResponsesContainer> GetFormResponsesAsync(
      [Authorize("Bearer")] string accessToken,
      [AliasAs("form_id")] string formId,
      TypeformResponsesParameters queryParams);
  }
}

public class TypeformResponsesContainer
{

  public TypeformResponsesContainer()
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

public class TypeformResponse
{
  public TypeformResponse()
  {
    Answers = new AnswerList();
  }

  public AnswerList Answers { get; set; }
}

public class AnswerList : List<TypeformAnswer>
{
  public T GetAnswer<T>(int index) where T : TypeformAnswer
  {
    return (T)this[index];
  }
}

public enum AnswerType
{
  Unknown,
  Text,
  Choice,
  Choices,
  Email,
  Url,
  [EnumMember(Value = "file_url")]
  FileUrl,
  Boolean,
  Number,
  Date,
  Payment
}

public enum QuestionType
{
  Matrix,
  Ranking,
  Date,
  Dropdown,
  Email,
  [EnumMember(Value = "file_upload")]
  FileUpload,
  Group,
  Legal,
  [EnumMember(Value = "long_text")]
  LongText,
  [EnumMember(Value = "multiple_choice")]
  MultipleChoice,
  Number,
  [EnumMember(Value = "opinion_scale")]
  OpinionScale,
  Payment,
  [EnumMember(Value = "picture_choice")]
  PictureChoice,
  Rating,
  [EnumMember(Value = "short_text")]
  ShortText,
  Statement,
  Website,
  [EnumMember(Value = "yes_no")]
  YesNo,
  [EnumMember(Value = "phone_number")]
  PhoneNumber
}

public class TypeformAnswer
{
  public TypeformAnswerField Field { get; set; }

  public AnswerType Type { get; set; }
}

public class TypeformTextAnswer : TypeformAnswer
{
  public string Text { get; set; }
}

public class TypeformBooleanAnswer : TypeformAnswer
{
  public bool Boolean { get; set; }
}

public class TypeformEmailAnswer : TypeformAnswer
{
  public string Email { get; set; }
}

public class TypeformFileUrlAnswer : TypeformAnswer
{

  /// <summary>
  /// TODO: Use Uri?
  /// </summary>
  /// <value></value>
  public Uri FileUrl { get; set; }
}

public class TypeformNumberAnswer : TypeformAnswer
{
  public int Number { get; set; }
}

public class TypeformChoicesAnswer : TypeformAnswer
{
  public TypeformChoicesLabels Choices { get; set; }
}

public class TypeformChoicesLabels
{
  public TypeformChoicesLabels()
  {
    Labels = new List<string>();
  }
  public IList<string> Labels { get; set; }
}

public class TypeformChoiceAnswer : TypeformAnswer
{
  public TypeformChoiceLabel Choice { get; set; }
}

public class TypeformChoiceLabel
{
  public string Label { get; set; }
}

public class TypeformDateAnswer : TypeformAnswer
{
  public DateTime Date { get; set; }
}

public class TypeformAnswerField
{
  public string Id { get; set; }

  public string Ref { get; set; }

  public string Type { get; set; }
}

public class TypeformResponsesParameters
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
  public string Fields { get; set; }

  /// <summary>
  /// Limit request to only responses that include the specified fields in 
  /// answers section. Use a comma-separated list to specify more than 
  /// one field value - response will contain at least one of the specified fields.
  /// </summary>
  /// <value></value>
  public string AnsweredFields { get; set; }
}