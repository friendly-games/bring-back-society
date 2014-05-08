using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
using Debug = UnityEngine.Debug;

public class DebugDraw : MonoBehaviour, IStartAndUpdate
{
  private LinkedList<Ray[]> _raysToDraw;
  private static DebugDraw Instance;

  /// <inheritdoc />
  public void Start()
  {
    _raysToDraw = new LinkedList<Ray[]>();
    Instance = this;
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
    Instance.StartCoroutine(Instance.DrawCoroutine(rays));
  }

  private IEnumerator DrawCoroutine(Ray[] rays)
  {
    _raysToDraw.AddLast(rays);
    yield return new WaitForSeconds(1);
    _raysToDraw.Remove(rays);
  }
}