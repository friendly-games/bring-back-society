using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items.Weapons;

namespace BringBackSociety.Items
{
  /// <summary> Represents an ammo type. </summary>
  public interface IAmmoModel : IItemModel
  {
    /// <summary> The type of ammo that this model represents. </summary>
    AmmoType AmmoType { get; }
  }
}