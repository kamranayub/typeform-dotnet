using System.Runtime.Serialization;

namespace Typeform;

public enum TypeformAnswerType
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
