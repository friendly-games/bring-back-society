using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Items
{
  /// <summary> The relative strength of an entity. </summary>
  [Serializable]
  public class Resistance
  {
    /// <summary> The entity's resistance to explosives. </summary>
    public float ExplosiveResistance = 1.0f;

    /// <summary> The entity's resistance to bullets. </summary>
    public float BulletResistance = 1.0f;
  }
}