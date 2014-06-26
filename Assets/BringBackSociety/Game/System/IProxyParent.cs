using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Engine.System;
using UnityEngine;

namespace BringBackSociety.Game.System
{
  /// <summary> A component which serves proxies for its children. </summary>
  internal interface IProxyParent
  {
    /// <summary> Retrieves a proxy for the given object, constructing it if needed. </summary>
    /// <param name="gameObject"> The game object for which a proxy should be constructed. </param>
    /// <returns> A proxy for the given game object. </returns>
    IThing RetrieveProxyFor(GameObject gameObject);
  }
}