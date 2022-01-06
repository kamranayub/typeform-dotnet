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

  /// <summary>
  /// Whether or not a variable exists with the given key
  /// </summary>
  /// <param name="key"></param>
  /// <returns></returns>
  public bool ContainsKey(string key) {
    return this.Any(v => v.Key == key);
  }
}
