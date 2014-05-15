using System;
using System.Collections.Generic;
using System.Linq;

namespace Behavior
{
  /// <summary> Represents an item that can be fired. </summary>
  [Serializable]
  public class Weapon
  {
    /// <summary> The name of the weapon. </summary>
    public string Name;

    /// <summary> The amount of damage done per bullet hit. </summary>
    public int DamagePerShot;

    /// <summary> The number of bullets in each clip. </summary>
    public int ClipSize = 3;

    /// <summary> The maximum distance the weapon can be fired </summary>
    public float MaxDistance = 100;
  }
}