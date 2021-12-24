namespace Typeform.Models;

public class TypeformResponseVariables : List<TypeformVariable>
{
  public T GetVariable<T>(int index) where T : TypeformVariable
  {
    return (T)this[index];
  }

  public T GetVariable<T>(string key) where T : TypeformVariable
  {
    return (T)this.FirstOrDefault(v => v.Key == key);
  }
}
