namespace Typeform.Models;

public enum TypeformVariableType
{
  Unknown,

  Number,

  Text,
}

public class TypeformVariable
{
  public string Key { get; set; }

  public TypeformVariableType Type { get; set; }
}

public class TypeformVariableNumber : TypeformVariable
{
  public int? Number { get; set; }
}

public class TypeformVariableText : TypeformVariable
{
  public string Text { get; set; }
}
