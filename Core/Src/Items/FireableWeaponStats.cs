﻿using BringBackSociety.Items.Weapons;

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
  }
}