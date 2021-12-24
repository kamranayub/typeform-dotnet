namespace Typeform.Models;

public class TypeformResponseAnswers : List<TypeformAnswer>
{
  public T GetAnswer<T>(int index) where T : TypeformAnswer
  {
    return (T)this[index];
  }
}
