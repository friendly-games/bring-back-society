using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using BringBackSociety.Maths;
using BringBackSociety.Services;
using Extensions;
using Items;
using UnityEngine;

namespace Services
{
  /// <summary> Unity implementation of the raycast service. </summary>
  internal class RaycastService : IRaycastService
  {
    /// <inheritdoc />
    T IRaycastService.Raycast<T>(IPlayer actor, float maxDistance)
    {
      var player = (Player) actor;
      return Raycast<T>(new Ray(player.Transform.position, player.Transform.forward), maxDistance);
    }

    T IRaycastService.Raycast<T>(ARay ray, float maxDistance)
    {
      Logging.Log.InfoFormat("Ray1: {0}", ray);

      return Raycast<T>(ray.ToRay(), maxDistance);
    }

    private T Raycast<T>(Ray ray, float maxDistance)
      where T : class, IComponent
    {
      RaycastHit hitInfo;

      Logging.Log.InfoFormat("Ray2: {0}", ray);
      if (!Physics.Raycast(ray, out hitInfo, maxDistance))
        return null;

      // automatically takes care of parents
      return hitInfo.collider.gameObject.RetrieveComponent<T>();
    }
  }
}