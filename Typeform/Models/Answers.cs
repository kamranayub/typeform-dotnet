using System.Runtime.Serialization;

namespace Typeform.Models;


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


public class TypeformAnswer
{
  public TypeformFieldReference Field { get; set; }

  public TypeformAnswerType Type { get; set; }
}


public class TypeformAnswerText : TypeformAnswer
{
  public string Text { get; set; }
}


public class TypeformAnswerBoolean : TypeformAnswer
{
  public bool Boolean { get; set; }
}

public class TypeformAnswerEmail : TypeformAnswer
{
  public string Email { get; set; }
}

public class TypeformAnswerFileUrl : TypeformAnswer
{
  public Uri FileUrl { get; set; }
}

public class TypeformAnswerUrl : TypeformAnswer
{
  public Uri Url { get; set; }
}

public class TypeformAnswerNumber : TypeformAnswer
{
  public int? Number { get; set; }
}

public class TypeformAnswerChoices : TypeformAnswer
{
  public TypeformAnswerChoicesData Choices { get; set; }
}

public class TypeformAnswerChoicesData
{
  public TypeformAnswerChoicesData()
  {
    Labels = new List<string>();
  }
  public IList<string> Labels { get; set; }

  public string Other { get; set; }
}

public class TypeformAnswerChoice : TypeformAnswer
{
  public TypeformAnswerChoiceData Choice { get; set; }
}

public class TypeformAnswerChoiceData
{
  public string Label { get; set; }

  public string Other { get; set; }
}

public class TypeformAnswerDate : TypeformAnswer
{
  public DateTime? Date { get; set; }
}

public class TypeformAnswerPayment : TypeformAnswer
{

  public TypeformAnswerPaymentData Payment { get; set; }
}

public class TypeformAnswerPaymentData
{
  public string Amount { get; set; }

  public string Last4 { get; set; }

  public string Name { get; set; }
}
