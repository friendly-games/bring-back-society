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

    /// <summary> The center of the screen. </summary>
    private readonly Vector2 _center;

    /// <summary> Constructor. </summary>
    /// <param name="width"> The width of the screen. </param>
    /// <param name="height"> The height of the screen. </param>
    /// <param name="innerRadius"> The inner radius where mouse movement does not count. </param>
    /// <param name="outerRadius"> The outer radius where mouse movement does count. </param>
    public TargetLocator(int width, int height, int innerRadius, int outerRadius)
    {
      _center = new Vector2(width / 2.0f, height / 2.0f);

      _innerRadius = innerRadius;
      _distanceToOuterRadius = outerRadius - _innerRadius;
    }

    /// <summary> The relative strength of the current direction of the mouse. Between 0 and 1. </summary>
    public float Strength { get; private set; }

    /// <summary>
    ///  The current rotation that would be applied to an object to rotate it in the same direction as
    ///  the mouse.
    /// </summary>
    public Quaternion Direction
    {
      get
      {
        return Quaternion.FromToRotation(Vector3.forward,
                                         new Vector3(EuelerDirection.x, 0, EuelerDirection.y));
      }
    }

    /// <summary>
    ///  The current relative distance from the center of the screen where the mouse currently is.
    /// </summary>
    public Vector2 EuelerDirection { get; private set; }

    /// <summary> The direction position. </summary>
    private Vector2 _directionPosition;

    /// <summary> The diff in position of the mouse from last time. </summary>
    /// <param name="diff"> The difference in position from the last update. </param>
    public void Update(Vector2 diff)
    {
      _directionPosition += diff;
      float relativeStrength = _directionPosition.magnitude - _innerRadius;

      // if our relative strength is less than zero, then we're in the inner circle
      // so we basically have no direction and no strength
      if (relativeStrength < 0)
      {
        Strength = 0;
        EuelerDirection = new Vector2(0, 0);
        return;
      }

      // make the magnitude of the vector doesn't exceed the outer circle
      if (relativeStrength > _distanceToOuterRadius)
      {
        _directionPosition = _directionPosition.normalized * _distanceToOuterRadius;
      }

      // otherwise the strength is a percentage based on how far close to the outer radius the position is
      Strength = relativeStrength / _distanceToOuterRadius;
      EuelerDirection = _directionPosition.normalized;
    }
  }
}