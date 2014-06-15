using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Controllers;
using BringBackSociety.Items;
using BringBackSociety.ViewModels;
using UnityEngine;

namespace BringBackSociety.Services
{
  /// <summary> A single living entity in the world. </summary>
  internal interface IActor
  {
    /// <summary> Gets the transform fro the player. </summary>
    Transform Transform { get; }
  }

  /// <summary> Represents a player of the game. </summary>
  internal interface IPlayer : IActor
  {
    /// <summary> The inventory of the actor. </summary>
    StorageContainer Inventory { get; }

    /// <summary> The selected slot of the player. </summary>
    StorageContainer.Cursor EquippedWeapon { get; }

    /// <summary> The host for the weapon that the player is holding. </summary>
    IModelHost<IFireableWeaponModel> WeaponHost { get; }
  }

  ///// <summary> Represents the inventory for a player. </summary>
  ///// TODO figure out if we need this
  //public interface IInventory
  //{
  //  /// <summary> Gets the owner of the player. </summary>
  //  IActor Owner { get; }

  //  /// <summary> Contains the current storage container for the inventory. </summary>
  //  StorageContainer Container { get; }

  //  /// <summary> Gets the current activatable item of the inventory. </summary>
  //  StorageContainer.Cursor Current { get; }
  //}

  ///// <summary> Manages the inventory for a player. </summary>
  ///// TODO figure out if we need this
  //public class InventoryManager
  //{
  //  /// <summary> The player's whose inventory should be managed. </summary>
  //  private readonly IActor _actor;

  //  public InventoryManager(IActor actor)
  //  {
  //    _actor = actor;
  //  }

  //  /// <summary> Activates the given quick slot for the player. </summary>
  //  /// <param name="slotId"> The numeric quick slot to activate. </param>
  //  public void ActivateQuickSlot(int slotId)
  //  {
  //  }

  //  /// <summary> Use the currently active item. </summary>
  //  public void Use()
  //  {
  //  }
  //}
}