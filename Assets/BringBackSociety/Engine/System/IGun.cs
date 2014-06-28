using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;

namespace BringBackSociety.Engine.System
{
  /// <summary> Data required for a gun. </summary>
  internal interface IGun : IAspect
  {
    /// <summary> The base stats for the weapon. </summary>
    GunStats BaseStats { get; }

    /// <summary> The number of shots remaining in the gun. </summary>
    int ShotsRemaining { get; set; }

    /// <summary> The model associated with the weapon. </summary>
    IFireableWeaponModel Model { get; }
  }
}