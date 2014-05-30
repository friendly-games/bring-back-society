using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace BringBackSociety.Items.Weapons
{
  /// <summary> An instance of a weapon that can be fired. </summary>
  internal class FireableWeapon : IItemModel
  {
    /// <summary> Constructor. </summary>
    public FireableWeapon(FireableWeaponStats stats)
    {
      Stats = stats;
    }

    /// <inheritdoc />
    public string Name
    {
      get { return "Gun"; }
    }

    public FireableWeaponStats Stats { get; private set; }

    /// <inheritdoc />
    public int StackAmount
    {
      // we can only ever hold 1 weapon in a slot at a time
      get { return 1; }
    }

    /// <inheritdoc />
    public IUiResource Resource
    {
      get { return null; }
    }

    /// <summary> The number of shots remaining until the clip needs to be refilled. </summary>
    public int ShotsRemaining { get; set; }
  }
}