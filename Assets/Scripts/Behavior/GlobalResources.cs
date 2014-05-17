using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using UnityEngine;

namespace Behavior
{
  /// <summary> All of the resources in the game (that can be edited from the editor). </summary>
  public class GlobalResources : MonoBehaviour
  {
    /// <summary> Provides logging for the class. </summary>
    private static readonly ILog Log = LogManager.GetLogger(typeof (GlobalResources));

    /// <summary> The singleton for this class. </summary>
    public static GlobalResources Instance { get; private set; }

    /// <summary> All of the weapons in the game. </summary>
    public Weapon[] Weapons;

    public static void Initialize(GlobalResources instance)
    {
      if (!ReferenceEquals(Instance, null))
      {
        Log.Fatal("Attempt to set Instance when it has already been set");
      }

      Log.Info("Set GlobalResources.Instance");
      Instance = instance;
    }
  }
}