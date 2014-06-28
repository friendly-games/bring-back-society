using System;
using System.Diagnostics;
using BringBackSociety.Engine.System;
using BringBackSociety.Items.Weapons;

namespace BringBackSociety.Items
{
  /// <summary> Stats about a fireable weapon. </summary>
  public struct FireableWeaponStats
  {
    /// <summary> The amount of damage done per bullet hit. </summary>
    public int DamagePerShot;

    /// <summary> The number of bullets in each clip. </summary>
    public int ClipSize;

    /// <summary> The maximum distance the weapon can be fired </summary>
    public float MaxDistance;

    /// <summary> The type of ammo that the weapon uses. </summary>
    public AmmoType AmmoType;

    /// <summary> The total number of pellets that the weapon fires. </summary>
    public int NumberOfPellets;

    /// <summary> The variance of each pellet that is fired. </summary>
    public float Spread;

    /// <summary> The distance after which the damage starts decreasing. </summary>
    public int FalloffDistance;

    /// <summary> Calculates the amount of base damage done by firing the weapon at a target a distance away. </summary>
    /// <param name="distance"> The distance to the target. </param>
    /// <returns> The calculated damage. </returns>
    public float CalculateDamage(float distance)
    {
      float damage = DamagePerShot;

      if (distance > FalloffDistance)
      {
        float falloffRange = MaxDistance - FalloffDistance;
        float curPositionInRange = distance - FalloffDistance;
        damage = DamagePerShot * (1 - curPositionInRange / falloffRange);
      }

      Debug.Assert(damage >= 0);

      // never do negative damage
      return Math.Max(damage, 0);
    }

    public static implicit operator GunStats(FireableWeaponStats stats)
    {
      return new GunStats()
             {
               ClipSize = stats.ClipSize,
               DamagePerShot = stats.DamagePerShot,
               FalloffDistance = stats.FalloffDistance,
               MaxDistance = stats.MaxDistance,
               NumberOfPellets = stats.NumberOfPellets,
               Spread = stats.Spread
             };
    }
  }
}