using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BringBackSociety.Items;

namespace BringBackSociety.Controllers
{
  /// <summary> A projectile weapon item. </summary>
  public class FireableWeaponInventoryItem
  {
    /// <summary> Constructor. </summary>
    /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
    /// <param name="fireableWeaponModel"> The weapon that this inventory item wraps. </param>
    public FireableWeaponInventoryItem(IFireableWeaponModel fireableWeaponModel)
    {
      if (fireableWeaponModel == null)
        throw new ArgumentNullException("fireableWeaponModel");

      FireableWeaponModel = fireableWeaponModel;
    }

    /// <summary> The weapon that this inventory item wraps. </summary>
    public IFireableWeaponModel FireableWeaponModel { get; private set; }

    /// <summary> The number of clips remaining </summary>
    public int ClipsRemaining { get; set; }

    /// <summary> The number of ammo units in the current clip. </summary>
    public int CurrentClipAmmoCount { get; set; }
  }
}