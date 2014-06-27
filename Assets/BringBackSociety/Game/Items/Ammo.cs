using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using BringBackSociety.Items.Weapons;

namespace Items
{
  /// <summary> Represents a type of ammunition in the game. </summary>
  [Serializable]
  public class Ammo : IAmmoModel
  {
    /// <inheritdoc />
    public string Name { get; set; }

    /// <inheritdoc />
    public int StackAmount { get; set; }

    /// <inheritdoc />
    public AmmoType AmmoType { get; set; }
  }
}