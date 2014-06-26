using System;
using System.Collections.Generic;
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

      Log.InfoFormat("Damaged {0}. Health remaining: {1}", hpHolder, hpHolder.Health);

      if (hpHolder.Health <= 0 && destroyable != null)
      {
        // mark as destroyed
        destroyable.MarkDestroyed();
      }
    }
  }
}