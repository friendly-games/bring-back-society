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
    public string name;
    public AmmoType ammoType;
    public int stackAmount;

    /// <inheritdoc />
    string INamedItem.Name
    {
      get { return name; }
    }

    /// <inheritdoc />
    int IItemModel.StackAmount
    {
      get { return stackAmount; }
    }

    /// <inheritdoc />
    IUiResource IDisplayableItem.Resource
    {
      get { return null; }
    }

    /// <inheritdoc />
    AmmoType IAmmoModel.AmmoType
    {
      get { return ammoType; }
    }
  }
}