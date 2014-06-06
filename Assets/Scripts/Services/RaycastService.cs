using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using BringBackSociety.Services;
using UnityEngine;

namespace Services
{
  /// <summary> Unity implementation of the raycast service. </summary>
  internal class RaycastService : IRaycastService
  {
    /// <inheritdoc />
    public T Raycast<T>(Ray ray, float maxDistance)
      where T : class, IComponent
    {
      float distance;
      return Raycast<T>(ray, maxDistance, out distance);
    }

    /// <inheritdoc />
    public T Raycast<T>(Ray ray, float maxDistance, out float distance)
      where T : class, IComponent
    {
      RaycastHit hitInfo;
      if (!Physics.Raycast(ray, out hitInfo, maxDistance))
      {
        distance = 0;
        return null;
      }

      distance = hitInfo.distance;

      // automatically takes care of parents
      return hitInfo.collider.gameObject.RetrieveComponent<T>();
    }
  }
}