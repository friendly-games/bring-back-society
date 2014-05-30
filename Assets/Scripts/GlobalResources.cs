using System;
using Behavior;
using BringBackSociety.Items;
using BringBackSociety.Items.Weapons;
using Items;
using log4net;
using UnityEngine;

public class GlobalResources : ScriptableObject
{
  /// <summary> Provides logging for the class. </summary>
  private static readonly ILog Log = LogManager.GetLogger(typeof(GlobalResources));

  /// <summary> The singleton for this class. </summary>
  public static GlobalResources Instance { get; private set; }

  /// <summary> The different types of ammo. </summary>
  public Ammo[] Ammos;

  public FireableWeaponStats[] WeaponStats { get; private set; }

  public GlobalResources()
  {
    WeaponStats = new[]
                  {
                    new FireableWeaponStats()
                    {
                      AmmoType = AmmoType.Pistol,
                      ClipSize = 10,
                      DamagePerShot = 50,
                      MaxDistance = 100,
                    },
                    new FireableWeaponStats()
                    {
                      AmmoType = AmmoType.Shotgun,
                      ClipSize = 4,
                      DamagePerShot = 20,
                      MaxDistance = 50,
                    },
                  };
  }

  public static void Initialize(GlobalResources instance)
  {
    if (instance == null)
      throw new ArgumentNullException("instance");

    if (!ReferenceEquals(Instance, null))
    {
      Log.Fatal("Attempt to set Instance when it has already been set");
    }

    Log.Info("Set GlobalResources.Instance");
    Instance = instance;
  }
}