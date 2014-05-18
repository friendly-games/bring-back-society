using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;

namespace Behavior
{
  /// <summary> Controls the behavior of killables. </summary>
  internal static class KillableController
  {
    /// <summary> Initialize a killable object. </summary>
    /// <param name="destroyable"> The killable to act on. </param>
    public static void Initialize(this IDestroyable destroyable)
    {
      destroyable.Health = destroyable.MaxHealth;
    }
  }
}