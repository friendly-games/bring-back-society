﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BringBackSociety.Extensions
{
  /// <summary> Extensions for converting to and from various types of coordinates. </summary>
  public static class CoordinateExtensions
  {
    /// <summary>
    ///  Convert a world position into a vector 3
    /// </summary>
    /// <param name="position"> The position to act on. </param>
    /// <returns> position as a Vector3. </returns>
    public static Vector3 ToVector3(this WorldPosition position)
    {
      return new Vector3(position.X + 0.5f, 0, position.Z + 0.5f);
    }

    /// <summary>
    ///  Convert a vector3 into a world position
    /// </summary>
    /// <param name="position"> The position to act on. </param>
    /// <returns> position as a WorldPosition. </returns>
    public static WorldPosition ToWorldPosition(this Vector3 position)
    {
      return new WorldPosition(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.z));
    }
  }
}