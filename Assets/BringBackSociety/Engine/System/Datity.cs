using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> A single entity in the world. </summary>
  public sealed class Datity : IDatity
  {
    private readonly Dictionary<ComponentInfo, IComponent> _components;

    /// <summary> Default constructor. </summary>
    public Datity()
    {
      _components = new Dictionary<ComponentInfo, IComponent>();
    }

    /// <inheritdoc />
    void IDatity.Put<T>(T component)
    {
      var info = ComponentInfo<T>.Info;

      // make sure we fullfill the requirements
      foreach (var requiredComponent in info.Requirements)
      {
        if (!_components.ContainsKey(requiredComponent))
        {
          throw new Exception(string.Format("Component {0} requires {1} but it is not present",
                                            info,
                                            requiredComponent));
        }
      }

      _components[info] = component;
    }

    /// <inheritdoc />
    T IDatity.Get<T>()
    {
      var info = ComponentInfo<T>.Info;
      IComponent value;
      if (_components.TryGetValue(info, out value))
        return (T) value;

      return null;
    }

    /// <summary> Makes a deep copy of this object. </summary>
    /// <returns> A copy of this object. </returns>
    public IDatity Clone()
    {
      var newDatity = new Datity();

      foreach (var entry in _components)
      {
        if (entry.Key.CanWrite)
        {
          newDatity._components[entry.Key] = ((IReadWriteComponent) entry.Value).Clone();
        }
        else
        {
          newDatity._components[entry.Key] = entry.Value;
        }
      }

      return newDatity;
    }
  }
}