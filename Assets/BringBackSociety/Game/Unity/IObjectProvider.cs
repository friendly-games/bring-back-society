using System;
using System.Collections.Generic;
using System.Linq;

namespace Behavior
{
  /// <summary> Provides a specific type of component. </summary>
  /// <typeparam name="T"> Generic type parameter. </typeparam>
  public interface IObjectProvider<out T>
  {
    /// <summary> Gets the component that can be provided. </summary>
    T Instance { get; }
  }
}