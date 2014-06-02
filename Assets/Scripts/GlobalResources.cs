using System;
using Behavior;
using BringBackSociety.Items;
using BringBackSociety.Items.Weapons;
using Items;
using UnityEngine;

/// <summary> All of the resources that are available in the game by default. </summary>
public class GlobalResources : ScriptableObject
{
  /// <summary> The different types of ammo. </summary>
  public static Ammo[] Ammos { get; private set; }

  /// <summary> The different weapons available by default. </summary>
  public static FireableWeaponStats[] WeaponStats { get; private set; }

  static GlobalResources()
  {
    WeaponStats = new[]
                  {
                    new FireableWeaponStats
                    {
                      AmmoType = AmmoType.Pistol,
                      ClipSize = 10,
                      DamagePerShot = 50,
                      MaxDistance = 100,
                    },
                    new FireableWeaponStats
                    {
                      AmmoType = AmmoType.Shotgun,
                      ClipSize = 4,
                      DamagePerShot = 20,
                      MaxDistance = 50,
                    },
                  };

    Ammos = new[]
            {
              new Ammo
              {
                AmmoType = AmmoType.Pistol,
                Name = "Pistol Ammo",
                StackAmount = 100,
              },
              new Ammo
              {
                AmmoType = AmmoType.Shotgun,
                Name = "Shotgun Ammo",
                StackAmount = 100,
              }
            };
  }
}