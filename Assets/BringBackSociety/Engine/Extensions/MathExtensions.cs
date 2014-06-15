using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BringBackSociety.Extensions
{
  /// <summary> Math-related extension methods. </summary>
  public static class MathExtensions
  {
    /// <summary>
    ///  Convert a transform into a ray based on the position and forward direction of the transform.
    /// </summary>
    /// <param name="transform"> The transform to act on. </param>
    /// <returns> The ray representing the position and forward direction of the transform. </returns>
    public static Ray ToRay(this Transform transform)
    {
      return new Ray(transform.position, transform.forward);
    }
  }
}