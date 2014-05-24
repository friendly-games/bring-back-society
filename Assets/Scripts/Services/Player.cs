using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Controllers;
using BringBackSociety.Services;
using JetBrains.Annotations;
using UnityEngine;

namespace Services
{
  /// <summary> Contains the root player. </summary>
  internal class Player : IPlayer
  {
    /// <summary> The game object that represents the physical player of the object. </summary>
    [UsedImplicitly]
    private readonly GameObject _gameObject;

    public Player(GameObject gameObject)
    {
      _gameObject = gameObject;
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
  }
}