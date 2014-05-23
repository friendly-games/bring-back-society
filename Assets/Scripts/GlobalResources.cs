using System;
using Behavior;
using Items;
using log4net;
using UnityEngine;

public class GlobalResources : ScriptableObject
{
  /// <summary> Provides logging for the class. </summary>
  private static readonly ILog Log = LogManager.GetLogger(typeof(GlobalResources));

  /// <summary> The singleton for this class. </summary>
  public static GlobalResources Instance { get; private set; }

  /// <summary> All of the weapons in the game. </summary>
  public Weapon[] Weapons;

  /// <summary> The different types of ammo. </summary>
  public Ammo[] Ammos;

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