using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Maths;
using UnityEngine;

namespace Extensions
{
  /// <summary> Extension methods to convert between various math types. </summary>
  internal static class MathExtensions
  {
    /// <summary> Convert a Vector3 to AVector3. </summary>
    public static AVector3 ToAVector3(this Vector3 vector)
    {
      return new AVector3(vector.x, vector.y, vector.z);
    }

    /// <summary> Convert a AVector3 to Vector3. </summary>
    public static Vector3 ToVector3(this AVector3 vector)
    {
      return new Vector3(vector.X, vector.Y, vector.Z);
    }

    /// <summary> Convert a ARay to Ray. </summary>
    public static Ray ToRay(this ARay aray)
    {
      return new Ray(aray.Position.ToVector3(), aray.Direction.ToVector3());
    }

    /// <summary> Convert a Ray to ARay. </summary>
    public static ARay ToARay(this Ray ray)
    {
      return new ARay(ray.origin.ToAVector3(), ray.direction.ToAVector3());
    }

    /// <summary> Convert a transform into an ARay. </summary>
    public static ARay ToARay(this Transform transform)
    {
      return new ARay(transform.position.ToAVector3(), transform.forward.ToAVector3());
    }
  }
}