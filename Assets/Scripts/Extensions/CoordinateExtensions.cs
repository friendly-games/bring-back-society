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
      return new Vector3(position.X / 2.0f, 0, position.Z / 2.0f);
    }

    /// <summary>
    ///  Convert a vector3 into a world position
    /// </summary>
    /// <param name="position"> The position to act on. </param>
    /// <returns> position as a WorldPosition. </returns>
    public static WorldPosition ToWorldPosition(this Vector3 position)
    {
      return new WorldPosition(Mathf.FloorToInt(position.x * 2), Mathf.FloorToInt(position.z * 2));
    }
  }
}