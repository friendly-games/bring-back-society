using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Behavior.Collidables
{
  /// <summary> An object, high up in the hierarchy of an object, that can handle collisions</summary>
  internal interface ICollisionHandler
  {
    /// <summary> Handles the collision against the other object. </summary>
    /// <param name="collision"> The collision type that occurred. </param>
    /// <param name="rhs"> The information about the collision. </param>
    void HandleCollision(CollisionType collision, Collider rhs);
  }
}