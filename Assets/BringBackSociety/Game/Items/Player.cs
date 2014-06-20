using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using BringBackSociety.Controllers;
using BringBackSociety.Items;
using BringBackSociety.Services;
using BringBackSociety.ViewModels;
using JetBrains.Annotations;
using Models;
using UnityEngine;

namespace Items
{
  /// <summary> Contains the root player. </summary>
  internal class Player : IPlayer
  {
    /// <summary> The game object that represents the physical player of the object. </summary>
    [UsedImplicitly]
    private readonly GameObject _gameObject;

    public Player(GameObject gameObject, ModelHost<IFireableWeaponModel> equippedItemHost)
    {
      _gameObject = gameObject;
      EquippedItemHost = equippedItemHost;

      Transform = gameObject.transform;

      Inventory = new StorageContainer(10);
      EquippedItem = Inventory.GetCursor(0);
    }

    /// <summary> The transform of the player. </summary>
    public Transform Transform { get; private set; }

    /// <inheritdoc />
    public StorageContainer Inventory { get; private set; }

    /// <inheritdoc />
    public StorageContainer.Cursor EquippedItem { get; set; }

    /// <inheritdoc />
    public IModelHost<IFireableWeaponModel> EquippedItemHost { get; private set; }
  }
}