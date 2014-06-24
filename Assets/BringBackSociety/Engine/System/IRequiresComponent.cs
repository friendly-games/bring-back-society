using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> An component that requires another component. </summary>
  /// <typeparam name="T"> The other component type that is required on this entity. </typeparam>
  public interface IRequiresComponent<T>
    where T : IComponent
  {
  }
}