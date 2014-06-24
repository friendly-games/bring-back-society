using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> Contains component info for the generic type T. </summary>
  /// <typeparam name="T"> The type of parameter to get information for. </typeparam>
  public static class ComponentInfo<T>
    where T : IComponent
  {
    // FYI, do not add a static constructor
    // http://msdn.microsoft.com/en-us/library/ms182275.aspx
    public static readonly ComponentInfo Info = new ComponentInfo(typeof(T));
  }

  /// <summary> Information about a given component. </summary>
  public class ComponentInfo : IEquatable<ComponentInfo>
  {
    private readonly Type _type;

    /// <summary> Constructor. </summary>
    /// <param name="type"> The type for when the component info should be loaded. </param>
    public ComponentInfo(Type type)
    {
      if (!type.IsInterface)
        throw new Exception("Type must be an interface");
      if (!typeof(IComponent).IsAssignableFrom(type))
        throw new Exception("Type must be a component type");

      _type = type;
      CanWrite = typeof(IReadWriteComponent).IsAssignableFrom(_type);

      if (!CanWrite && _type.GetProperties().Any(p => p.CanWrite))
      {
        var writableProperty = _type.GetProperties().First(p => p.CanWrite);
        throw new Exception("Property " + writableProperty +
                            " is writable but type does not implement IReadWriteComponent");
      }
      else if (CanWrite && _type.GetProperties().Any(p => !p.CanWrite))
      {
        var writableProperty = _type.GetProperties().First(p => p.CanWrite);
        throw new Exception("Property " + writableProperty +
                            " is not writable but type does implement IReadWriteComponent");
      }

      var required = new List<ComponentInfo>();

      foreach (var interf in type.GetInterfaces())
      {
        if (interf.IsGenericType && interf.GetGenericTypeDefinition() == typeof(IRequiresComponent<>))
        {
          var info = (ComponentInfo) typeof(ComponentInfo<>).MakeGenericType(interf.GetGenericArguments()[0])
                                                            .GetField("Info").GetValue(null);

          required.Add(info);
        }
      }

      Requirements = required.ToArray();
    }

    /// <summary> The required components for this component. </summary>
    public ComponentInfo[] Requirements { get; private set; }

    /// <summary> True if this component can be written to in addition to being able to be read. </summary>
    public bool CanWrite { get; private set; }

    /// <inheritdoc />
    public bool Equals(ComponentInfo other)
    {
      if (ReferenceEquals(null, other))
        return false;
      if (ReferenceEquals(this, other))
        return true;

      return _type == other._type;
    }

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      return Equals(obj as ComponentInfo);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      return (_type != null ? _type.GetHashCode() : 0);
    }

    /// <inheritdoc />
    public static bool operator ==(ComponentInfo left, ComponentInfo right)
    {
      return Equals(left, right);
    }

    /// <inheritdoc />
    public static bool operator !=(ComponentInfo left, ComponentInfo right)
    {
      return !Equals(left, right);
    }
  }
}