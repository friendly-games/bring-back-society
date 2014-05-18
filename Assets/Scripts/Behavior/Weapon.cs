using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;

namespace Behavior
{
  /// <summary> Represents an item that can be fired. </summary>
  [Serializable]
  public class Weapon : IFireableWeaponModel
  {
    /// <summary> The name of the weapon. </summary>
    public string name;

    /// <summary> The amount of damage done per bullet hit. </summary>
    public int damagePerShot;

    /// <summary> The number of bullets in each clip. </summary>
    public int clipSize = 3;

    /// <summary> The maximum distance the weapon can be fired </summary>
    public float maxDistance = 100;

    /// <inheritdoc />
    string IFireableWeaponModel.Name
    {
      get { return name; }
    }

    /// <inheritdoc />
    int IFireableWeaponModel.DamagePerShot
    {
      get { return damagePerShot; }
    }

    /// <inheritdoc />
    int IFireableWeaponModel.ClipSize
    {
      get { return clipSize; }
    }

    /// <inheritdoc />
    float IFireableWeaponModel.MaxDistance
    {
      get { return maxDistance; }
    }
  }
}