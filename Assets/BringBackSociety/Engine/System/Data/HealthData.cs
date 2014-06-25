using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> An instance of the health component. </summary>
  internal class HealthData : SimpleComponent<IHealth>, IHealth
  {
    /// <summary> Constructor. </summary>
    /// <exception cref="ArgumentException"> Thrown when one or more arguments have unsupported or
    ///  illegal values. </exception>
    /// <param name="maxHealth"> The maximum health the entity can have. </param>
    /// <param name="initialHealth"> (Optional) the initial health of the entity. </param>
    public HealthData(int maxHealth, int initialHealth = 0)
    {
      if (maxHealth <= 0)
        throw new ArgumentException("maxHealth must be > 0", "maxHealth");
      if (initialHealth < 0)
        throw new ArgumentException("initialHealth must be >= 0", "initialHealth");

      MaxHealth = maxHealth;
      Health = initialHealth;
    }

    /// <inheritdoc />
    public int Health { get; set; }

    /// <inheritdoc />
    public int MaxHealth { get; private set; }

    /// <inheritdoc />
    public IReadWriteComponent Clone()
    {
      return new HealthData(MaxHealth, Health);
    }
  }
}