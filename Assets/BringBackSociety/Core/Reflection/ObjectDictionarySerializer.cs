using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BringBackSociety.Core.Reflection
{
  /// <summary> Non generic class of ObjectDictionarySerializer[T] </summary>
  internal abstract class ObjectDictionarySerializer
  {
    private readonly List<PropertyInfo> _properties;

    /// <summary> Specialised constructor for use only by derived classes. </summary>
    protected ObjectDictionarySerializer(Type type)
    {
      if (type == null)
        throw new ArgumentNullException("type");

      _properties = type.GetProperties()
                        .ToList();
    }

    /// <summary> Serialize the object so that it can be displayed. </summary>
    /// <param name="instance"> The instance whose data should be processed. </param>
    /// <returns> The names and values of each property in the object. </returns>
    protected List<KeyValuePair<string, string>> Serialize(object instance)
    {
      if (instance == null)
        throw new ArgumentNullException("instance");

      var keyValuePairs = new List<KeyValuePair<string, string>>();

      foreach (var property in _properties)
      {
        var keyValuePair = new KeyValuePair<string, string>(property.Name,
                                                            property.GetValue(instance, null).ToString());
        keyValuePairs.Add(keyValuePair);
      }

      return keyValuePairs;
    }
  }

  /// <summary> Converts an object into a dictionary of values. </summary>
  internal class ObjectDictionarySerializer<T> : ObjectDictionarySerializer
  {
    public ObjectDictionarySerializer()
      : base(typeof(T))
    {
    }

    /// <summary> Serialize the object so that it can be displayed. </summary>
    /// <param name="instance"> The instance whose data should be processed. </param>
    /// <returns> The names and values of each property in the object. </returns>
    public List<KeyValuePair<string, string>> Serialize(T instance)
    {
      return base.Serialize(instance);
    }
  }
}