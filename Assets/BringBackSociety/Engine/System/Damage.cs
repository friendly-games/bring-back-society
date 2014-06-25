using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> Represents damage to be applied to some object. </summary>
  internal struct Damage
  {
    private readonly int _amount;

    /// <summary> Constructor. </summary>
    /// <param name="amount"> The amount of damage to apply. </param>
    public Damage(int amount)
    {
      _amount = amount;
    }

    /// <summary> The amount of damage that should be applied to an object. </summary>
    public int Amount
    {
      get { return _amount; }
    }
  }
}