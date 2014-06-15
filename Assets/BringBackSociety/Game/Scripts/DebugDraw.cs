using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
  /// <summary> Performs intermediate debug drawing for various actions. </summary>
  /// <remarks> For development only. </remarks>
  public class DebugDraw : MonoBehaviour, IStartAndUpdate
  {
    private LinkedList<Ray[]> _raysToDraw;
    private static DebugDraw _instance;

    /// <inheritdoc />
    public void Start()
    {
      _raysToDraw = new LinkedList<Ray[]>();
      _instance = this;
    }

    /// <inheritdoc />
    public void Update()
    {
      foreach (var rays in _raysToDraw)
      {
        foreach (var ray in rays)
        {
          Debug.DrawRay(ray.origin, ray.direction);
        }
      }
    }

    /// <summary> Draws the given ray. </summary>
    /// <param name="ray"> The ray to draw. </param>
    public static void Draw(Ray ray)
    {
      Debug.Log("Draw");
      Draw(new[] {ray});
    }

    /// <summary> Draws the given ray. </summary>
    /// <param name="ray"> The ray to draw. </param>
    public static void Draw(Ray[] rays)
    {
      _instance.StartCoroutine(_instance.DrawCoroutine(rays));
    }

    private IEnumerator DrawCoroutine(Ray[] rays)
    {
      _raysToDraw.AddLast(rays);
      yield return new WaitForSeconds(1);
      _raysToDraw.Remove(rays);
    }
  }
}