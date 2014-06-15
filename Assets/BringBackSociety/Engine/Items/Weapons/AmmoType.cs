using System;
using System.Collections.Generic;
using System.Linq;

namespace BringBackSociety.Items.Weapons
{
  /// <summary> The type of ammo that a projectile weapon takes. </summary>
  public enum AmmoType
  {
    /// <summary> The weapon does not use ammunition. </summary>
    None,

    /// <summary> The weapon uses Pistol ammunition. </summary>
    Pistol,

    /// <summary> The weapon uses Shotgun ammunition. </summary>
    Shotgun,
  }
}