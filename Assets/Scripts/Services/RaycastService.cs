using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Services;
using UnityEngine;

namespace Services
{
  /// <summary> Unity implementation of the raycast service. </summary>
  internal class RaycastService : IRaycastService
  {
    /// <inheritdoc />
    T IRaycastService.Raycast<T>(IPlayer actor, float maxDistance)
    {
      // TODO don't assume player
      var player = (Player) actor;

      RaycastHit hitInfo;
      if (!Physics.Raycast(new Ray(player.Transform.position, player.Transform.forward),
                           out hitInfo,
                           maxDistance))
        return null;

      // automatically takes care of parents
      return hitInfo.collider.gameObject.RetrieveComponent<T>();
    }
  }
}