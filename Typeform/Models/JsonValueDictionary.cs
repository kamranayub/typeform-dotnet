namespace Typeform.Models;

public class TypeformJsonValueDictionary : Dictionary<string, JsonElement>
{
  public T Value<T>(string key)
  {
    if (typeof(T).IsAssignableFrom(typeof(String)))
    {
      return (T)(object)this[key].GetString();
    }
    else if (typeof(T).IsAssignableFrom(typeof(Boolean)))
    {
      return (T)(object)this[key].GetBoolean();
    }
    else if (typeof(T).IsAssignableFrom(typeof(Int32)))
    {
      return (T)(object)this[key].GetInt32();
    }
    else if (typeof(T).IsAssignableFrom(typeof(Int16)))
    {
      return (T)(object)this[key].GetInt16();
    }
    else if (typeof(T).IsAssignableFrom(typeof(Int64)))
    {
      return (T)(object)this[key].GetInt64();
    }
    else if (typeof(T).IsAssignableFrom(typeof(DateTime)))
    {
      return (T)(object)this[key].GetDateTime();
    }
    else if (typeof(T).IsAssignableFrom(typeof(DateTimeOffset)))
    {
      return (T)(object)this[key].GetDateTimeOffset();
    }
    else if (typeof(T).IsAssignableFrom(typeof(Guid)))
    {
      return (T)(object)this[key].GetGuid();
    }
    else if (typeof(T).IsAssignableFrom(typeof(SByte)))
    {
      return (T)(object)this[key].GetSByte();
    }
    else if (typeof(T).IsAssignableFrom(typeof(Single)))
    {
      return (T)(object)this[key].GetSingle();
    }
    else if (typeof(T).IsAssignableFrom(typeof(Decimal)))
    {
      return (T)(object)this[key].GetDecimal();
    }

    throw new InvalidOperationException($"Unsupported type {typeof(T)} to deserialize from JSON");
  }
}
