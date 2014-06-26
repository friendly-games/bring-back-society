using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Engine.System;
using BringBackSociety.Items;
using BringBackSociety.Services;
using UnityEngine;

namespace Services
{
  /// <summary> Unity implementation of the raycast service. </summary>
  internal class RaycastService : IRaycastService
  {
    /// <inheritdoc />
    public IThing Raycast(Ray ray, float maxDistance, out float distance)
    {
      RaycastHit hitInfo;
      if (!Physics.Raycast(ray, out hitInfo, maxDistance))
      {
        distance = 0;
        return null;
      }

      distance = hitInfo.distance;

      // automatically takes care of parents
      return hitInfo.collider.gameObject.RetrieveThing();
    }
  }
}