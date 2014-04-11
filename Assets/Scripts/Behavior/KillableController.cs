using System;
using System.Collections.Generic;
using System.Linq;

namespace Behavior
{
  /// <summary> Controls the behavior of killables. </summary>
  internal static class KillableController
  {
    /// <summary> Initialize a killable object. </summary>
    /// <param name="killable"> The killable to act on. </param>
    public static void Initialize(this IKillable killable)
    {
      killable.Health = killable.MaxHealth;
    }

    /// <summary> Perform damage on the killable object </summary>
    /// <param name="killable"> The killable to act on. </param>
    /// <param name="damageAmount">The amount of damage to perform. </param>
    public static void Damage(this IKillable killable, int damageAmount)
    {
      killable.Health -= (int) (damageAmount/killable.Resistance.BulletResistance);

      if (killable.Health <= 0)
      {
        killable.Destroy();
      }
    }
  }
}