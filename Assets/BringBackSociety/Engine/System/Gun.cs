using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;

namespace BringBackSociety.Engine.System
{
  /// <summary> Represents a single gun in the world. </summary>
  internal class Gun : IGun
  {
    /// <summary> Constructor. </summary>
    /// <param name="stats"> The stats for the gun. </param>
    /// <param name="model"> The graphics model for the gun. </param>
    public Gun(GunStats stats, IFireableWeaponModel model)
    {
      BaseStats = stats;
      Model = model;
    }

    /// <inheritdoc />
    public GunStats BaseStats { get; private set; }

    /// <inheritdoc />
    public IFireableWeaponModel Model { get; private set; }

    /// <inheritdoc />
    public int ShotsRemaining { get; set; }
  }
}