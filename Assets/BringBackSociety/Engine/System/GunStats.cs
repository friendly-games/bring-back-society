using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Engine.System
{
  /// <summary> Stats about a fireable weapon. </summary>
  public struct GunStats
  {
    /// <summary> The amount of damage done per bullet hit. </summary>
    public int DamagePerShot;

    /// <summary> The number of bullets in each clip. </summary>
    public int ClipSize;

    /// <summary> The maximum distance the weapon can be fired </summary>
    public float MaxDistance;

    /// <summary> The total number of pellets that the weapon fires. </summary>
    public int NumberOfPellets;

    /// <summary> The variance of each pellet that is fired. </summary>
    public float Spread;

    /// <summary> The distance after which the damage starts decreasing. </summary>
    public int FalloffDistance;
  }
}