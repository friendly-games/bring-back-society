using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BringBackSociety.Game.Input
{
  /// <summary> Handles relative mouse movement from the player. </summary>
  /// <remarks> Mouse movement counts between innerRadius and outerRadius. </remarks>
  public class TargetLocator
  {
    /// <summary> The outer radius where mouse movement does count. </summary>
    private readonly int _distanceToOuterRadius;

    /// <summary> The inner radius where mouse movement does not count </summary>
    private readonly int _innerRadius;

    /// <summary> Constructor. </summary>
    /// <param name="width"> The width of the screen. </param>
    /// <param name="height"> The height of the screen. </param>
    /// <param name="innerRadius"> The inner radius where mouse movement does not count. </param>
    /// <param name="outerRadius"> The outer radius where mouse movement does count. </param>
    public TargetLocator(int width, int height, int innerRadius, int outerRadius)
    {
      Center = new Vector2(width / 2.0f, height / 2.0f);

      _innerRadius = innerRadius;
      _distanceToOuterRadius = outerRadius - _innerRadius;
    }

    /// <summary> The relative strength of the current direction of the mouse. Between 0 and 100. </summary>
    public float Strength { get; private set; }

    /// <summary>
    ///  The current rotation that would be applied to an object to rotate it in the same direction as
    ///  the mouse.
    /// </summary>
    public Quaternion Direction { get; private set; }

    /// <summary>
    ///  The current relative distance from the center of the screen where the mouse currently is.
    /// </summary>
    public Vector2 CurrentPosition { get; private set; }

    /// <summary> The center of the screen. </summary>
    public Vector2 Center { get; private set; }

    /// <summary> Updates the location of the mouse. </summary>
    /// <param name="location"> The location of the mouse in screen coordinates. </param>
    public void UpdatePosition(Vector2 location)
    {
      var ray = location - Center;
      float distance = ray.magnitude;

      if (distance < _innerRadius)
      {
        // we're within the inner circle, so don't change position at call
        Strength = 0;
        CurrentPosition = new Vector2(0, 0);
        return;
      }

      // otherwise the strength is a percentage based on how far close to the outer radius the position is
      var strength = (Mathf.Min(_distanceToOuterRadius, distance - _innerRadius))
                     / (_distanceToOuterRadius);

      CurrentPosition = ray.normalized;

      Strength = strength * 100;
      Direction = Quaternion.FromToRotation(Vector3.forward, new Vector3(ray.x, 0, ray.y));
    }
  }
}