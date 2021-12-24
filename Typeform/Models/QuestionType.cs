using System.Runtime.Serialization;

namespace Typeform.Models;

public enum TypeformQuestionType
{
  Unknown,
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
