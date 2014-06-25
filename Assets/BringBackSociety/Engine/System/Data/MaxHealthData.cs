using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> Represents the maximum health of an object. </summary>
  internal class MaxHealthData : SimpleComponent<IHealable>, IHealable
  {
    /// <summary> Constructor. </summary>
    /// <param name="maxHealth"> The maximum amount of health that this object can have. </param>
    public MaxHealthData(int maxHealth)
    {
      MaxHealth = maxHealth;
    }

    /// <summary> The maximum amount of health that this object can have. </summary>
    public int MaxHealth { get; private set; }
  }
}