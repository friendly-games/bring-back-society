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

    /// <summary> Debug drawing routines. </summary>
    public static IDebugDraw Drawing { get; private set; }

    /// <summary> Draw a ray. </summary>
    internal interface IDebugDraw
    {
      /// <summary> Draws a single ray. </summary>
      /// <param name="ray"> The ray to draw. </param>
      /// <param name="distance"> The distance to draw the ray. </param>
      /// <param name="time"> (Optional) The amount of time for which the ray should be visible. </param>
      /// <param name="color"> (Optional) the color to draw the ray. </param>
      void Draw(Ray ray, float distance, float time = 1.5f, Color? color = null);
    }

    /// <summary> Draws rays using the unity functions. </summary>
    internal class DebugDrawing : IDebugDraw
    {
      /// <inheritdoc />
      void IDebugDraw.Draw(Ray ray, float distance, float time, Color? color)
      {
        Debug.DrawRay(ray.origin, ray.direction * distance, color ?? Color.white, time);
      }
    }
  }
}