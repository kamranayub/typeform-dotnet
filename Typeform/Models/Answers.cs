namespace Typeform.Models;

public class TypeformAnswer
{
  public TypeformAnswerField Field { get; set; }

  public TypeformAnswerType Type { get; set; }
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
  public Uri FileUrl { get; set; }
}

public class TypeformUrlAnswer : TypeformAnswer
{
  public Uri Url { get; set; }
}

public class TypeformNumberAnswer : TypeformAnswer
{
  public int? Number { get; set; }
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

  public string Other { get; set; }
}

public class TypeformChoiceAnswer : TypeformAnswer
{
  public TypeformChoiceLabel Choice { get; set; }
}

public class TypeformChoiceLabel
{
  public string Label { get; set; }

  public string Other { get; set; }
}

public class TypeformDateAnswer : TypeformAnswer
{
  public DateTime? Date { get; set; }
}

public class TypeformPaymentAnswer : TypeformAnswer
{

  public TypeformPaymentAnswerData Payment { get; set; }
}

public class TypeformPaymentAnswerData
{
  public string Amount { get; set; }

  public string Last4 { get; set; }

  public string Name { get; set; }
}
