using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety;
using UnityEngine;

namespace Extensions
{
  public static class CoordinateExtensions
  {
    /// <summary>
    ///  Convert a world position into a vector 3
    /// </summary>
    /// <param name="position"> The position to act on. </param>
    /// <returns> position as a Vector3. </returns>
    public static Vector3 ToVector3(this WorldPosition position)
    {
      return new Vector3(position.X, 0, position.Z);
    }
  }
}