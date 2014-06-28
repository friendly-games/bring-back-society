using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BringBackSociety.Items;
using log4net;

namespace BringBackSociety.Engine.System
{
  /// <summary>
  ///  Calculates the damage applied to a thing.  Handles object destruction if HP is &lt;= 0;
  /// </summary>
  internal class DamageSystem
  {
    /// <summary> Provides logging for the class. </summary>
    private static readonly ILog Log = LogManager.GetLogger(typeof(DamageSystem));

    /// <summary> Applies damage to the given thing. </summary>
    /// <param name="stats"> The stats used to cause the damage. </param>
    /// <param name="distance"> The distance to thing </param>
    /// <param name="thing"> The thing that could be damaged. </param>
    public void ApplyDamage(GunStats stats, float distance, IThing thing)
    {
      var damage = new Damage((int) CalculateDamage(stats, distance));
      ApplyDamage(damage, thing);
    }

    /// <summary> Applies damage to the given thing. </summary>
    /// <param name="damage"> The damage to apply. </param>
    /// <param name="thing"> The thing that could be damaged. </param>
    public void ApplyDamage(Damage damage, IThing thing)
    {
      var hpHolder = thing as IHpHolder;
      var destroyable = thing as ICanBeDestroyed;
      var resistanceProvider = thing as IResist;

      if (hpHolder == null)
        return;

      var resistance = resistanceProvider == null
        ? Resistance.Default
        : resistanceProvider.Resistance;

      hpHolder.Health -= damage.Amount;

      Log.InfoFormat("Damaged {0} with {2}. Health remaining: {1}", hpHolder, hpHolder.Health, damage.Amount);

      if (hpHolder.Health <= 0 && destroyable != null)
      {
        // mark as destroyed
        destroyable.MarkDestroyed();
      }
    }

    /// <summary>
    ///  Calculates the amount of base damage done by firing the weapon at a target a distance away.
    /// </summary>
    /// <param name="stats"> The stats of which to base the damage off of.. </param>
    /// <param name="distance"> The distance to the target. </param>
    /// <returns> The calculated damage. </returns>
    private float CalculateDamage(GunStats stats, float distance)
    {
      float damage = stats.DamagePerShot;

      if (distance > stats.FalloffDistance)
      {
        float falloffRange = stats.MaxDistance - stats.FalloffDistance;
        float curPositionInRange = distance - stats.FalloffDistance;
        damage = stats.DamagePerShot * (1 - curPositionInRange / falloffRange);
      }

      Debug.Assert(damage >= 0);

      // never do negative damage
      return Math.Max(damage, 0);
    }
  }
}