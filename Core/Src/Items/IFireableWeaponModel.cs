using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items.Weapons;

namespace BringBackSociety.Items
{
  /// <summary> A single weapon which can be fired against an enemy. </summary>
  public interface IFireableWeaponModel : IDisplayableItem, INamedItem
  {
    /// <summary> The amount of damage done per bullet hit. </summary>
    int DamagePerShot { get; }

    /// <summary> The number of bullets in each clip. </summary>
    int ClipSize { get; }

    /// <summary> The maximum distance the weapon can be fired </summary>
    float MaxDistance { get; }

    /// <summary> The type of ammo that the weapon uses. </summary>
    AmmoType AmmoType { get; }
  }
}