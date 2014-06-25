using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> Implements a component that already has GetComonentInfo() implemented. </summary>
  /// <typeparam name="T"> Generic type parameter. </typeparam>
  internal abstract class SimpleComponent<T> : IComponent
    where T : IComponent
  {
    /// <inheritdoc />
    public ComponentInfo GetComponentInfo()
    {
      return ComponentInfo<T>.Info;
    }
  }
}