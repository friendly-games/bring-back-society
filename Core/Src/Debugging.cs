using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Services;
using UnityEngine;

namespace BringBackSociety
{
  /// <summary> A debugging. </summary>
  internal static class Debugging
  {
    static Debugging()
    {
      Drawing = new DebugDrawing();
    }

    public static IDebugDraw Drawing { get; private set; }

    /// <summary> Draw a ray. </summary>
    internal interface IDebugDraw
    {
      /// <summary> Draws a single ray. </summary>
      /// <param name="ray"> The ray to draw. </param>
      /// <param name="distance"> The distance to draw the ray. </param>
      void Draw(Ray ray, float distance);
    }

    internal class DebugDrawing : IDebugDraw
    {
      void IDebugDraw.Draw(Ray ray, float distance)
      {
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red, 1.5f);
      }
    }
  }
}