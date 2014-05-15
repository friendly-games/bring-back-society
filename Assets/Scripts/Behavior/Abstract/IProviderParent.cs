using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Behavior
{
  /// <summary> An object that provides some sort of interface for it's children. </summary>
  /// <typeparam name="T"> The type of interface that the parent provides. </typeparam>
  public interface IProviderParent<T>
  {
    /// <summary> Query if the designated child is applicable for this interface. </summary>
    /// <param name="child"> The child to check if it is applicable. </param>
    bool IsApplicable(GameObject child);

    /// <summary> Activate the Implementation property to be active with the designated child. </summary>
    /// <remarks>
    ///  This allows one parent to service multiple children, by reusing the Implementation object below.
    /// </remarks>
    /// <param name="child"> The child to have the Implementation property operate on. </param>
    void With(GameObject child);

    /// <summary> Gets the Implementation with the values for the given child object. </summary>
    T Implementation { get; }
  }
}