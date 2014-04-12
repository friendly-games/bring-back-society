using System;
using System.Collections.Generic;
using System.Linq;

namespace Behavior
{
  /// <summary> Represents an item that can be fired. </summary>
  [Serializable]
  public class Weapon
  {
    public string Name;

    public int DamagePerShot;

    public int ClipSize;
  }
}