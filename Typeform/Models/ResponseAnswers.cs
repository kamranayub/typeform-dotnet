namespace Typeform.Models;

public class TypeformResponseAnswers : List<TypeformAnswer>
{

  /// <summary>
  /// Gets answer by index
  /// </summary>
  /// <param name="index"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  public T GetAnswer<T>(int index) where T : TypeformAnswer
  {
    return (T)this[index];
  }

  /// <summary>
  /// Gets answer by `field.id` value
  /// </summary>
  /// <param name="id"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  public T GetAnswerById<T>(string id) where T : TypeformAnswer
  {
    return (T)this.FirstOrDefault(answer => answer.Field.Id == id);
  }

  /// <summary>
  /// Gets answer by `field.ref` value
  /// </summary>
  /// <param name="refName"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  public T GetAnswerByRef<T>(string refName) where T : TypeformAnswer
  {
    return (T)this.FirstOrDefault(answer => answer.Field.Ref == refName);
  }
}
