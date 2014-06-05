using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using BringBackSociety.Controllers;
using BringBackSociety.Items;
using BringBackSociety.Maths;
using BringBackSociety.Services;
using BringBackSociety.ViewModels;
using Extensions;
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

    public Player(GameObject gameObject, ModelHost<IFireableWeaponModel> weaponHost)
    {
      _gameObject = gameObject;
      WeaponHost = weaponHost;

      Transform = gameObject.transform;

      Inventory = new StorageContainer(10);
      EquippedWeapon = Inventory.GetCursor(0);
    }

    /// <summary> The transform of the player. </summary>
    public Transform Transform { get; private set; }

    /// <inheritdoc />
    public StorageContainer Inventory { get; private set; }

    /// <inheritdoc />
    public StorageContainer.Cursor EquippedWeapon { get; set; }

    /// <inheritdoc />
    public ARay Position
    {
      get { return Transform.ToARay(); }
    }

    /// <inheritdoc />
    public IModelHost<IFireableWeaponModel> WeaponHost { get; private set; }
  }
}