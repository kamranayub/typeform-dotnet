namespace Typeform.Models;

public class TypeformFieldReference
{
  /// <summary>
  /// The unique id of the form field the answer refers to.
  /// </summary>
  /// <value></value>
  public string Id { get; set; }

  /// <summary>
  /// The reference for the question the answer relates to. Use the `ref` value to 
  /// match answers with questions. The Responses payload only includes `ref` 
  /// for the fields where you specified them when you created the form.
  /// </summary>
  /// <value></value>
  public string Ref { get; set; }

  /// <summary>
  /// The field's type in the original form.
  /// </summary>
  /// <value></value>
  public TypeformQuestionType Type { get; set; }

  /// <summary>
  /// The form field's title which the answer is related to.
  /// </summary>
  /// <value></value>
  /// <remarks>May not be used</remarks>
  public string Title { get; set; }
}
