using System;
using System.Collections.Generic;
using System.Linq;
using Behavior;
using BringBackSociety.Controllers;
using UnityEngine;

namespace BringBackSociety.Scripts
{
  /// <summary> Holds an item that can be picked up by a player. </summary>
  public class ItemPickupBehavior : MonoBehaviour, IObjectProvider<InventoryStack>, IStart
  {
    public void Start()
    {
      // TODO change this to something more than just the instance data
      Instance = new InventoryStack(GlobalResources.Ammos[2], 20);
    }

    /// <summary> The data that will be picked up by the player. </summary>
    public InventoryStack Instance { get; set; }
  }
}