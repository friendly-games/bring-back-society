using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Engine.System;
using BringBackSociety.Extensions;
using BringBackSociety.Items;
using BringBackSociety.Items.Weapons;
using BringBackSociety.Services;
using log4net;
using UnityEngine;

namespace BringBackSociety.Controllers
{
  /// <summary> Handles firing of projectile weapons. </summary>
  internal class FireableWeaponController
  {
    /// <summary> Provides logging for the class. </summary>
    private static readonly ILog Log = LogManager.GetLogger(typeof(FireableWeaponController));

    private readonly IRaycastService _raycastService;
    private readonly IRandomNumberGenerator _randomNumberGenerator;
    private readonly DamageSystem _damageSystem;

    /// <summary> Constructor. </summary>
    public FireableWeaponController(IRaycastService raycastService,
                                    IRandomNumberGenerator randomNumberGenerator)
    {
      _raycastService = raycastService;
      _randomNumberGenerator = randomNumberGenerator;
      _damageSystem = new DamageSystem();
    }

    /// <summary> Have the actor fire their primary weapon. </summary>
    /// <param name="player"> The actor who should fire their weapon. </param>
    /// <param name="weapon"> The weapon that should be fired. </param>
    /// <returns> The result of firing the weapon </returns>
    public FireResult FireWeapon(IPlayer player, IGun weapon)
    {
      if (weapon.ShotsRemaining == 0)
        return FireResult.OutOfAmmo;

      var result = FireResult.Missed;

      var stats = weapon.BaseStats;

      var right = player.Transform.right;
      var up = player.Transform.up;

      for (int i = 0; i < stats.NumberOfPellets; i++)
      {
        var ray = player.Transform.ToRay();

        if (stats.Spread > 0)
        {
          var offset = right * _randomNumberGenerator.NextFloat(-stats.Spread, stats.Spread)
                       + up * _randomNumberGenerator.NextFloat(-stats.Spread, stats.Spread);

          ray.direction += offset;
        }

        float distance;
        var thing = _raycastService.Raycast(ray, stats.MaxDistance, out distance);

        Debugging.Drawing.Draw(ray, distance);

        if (thing != null)
        {
          var damageSystem = new DamageSystem();
          damageSystem.ApplyDamage(weapon.BaseStats, distance, thing);

          result = FireResult.Hit;
        }
      }

      weapon.ShotsRemaining--;

      return result;
    }

    /// <summary> Reloads a weapon from inventory. </summary>
    /// <param name="inventory"> The inventory to reload the weapon with. </param>
    /// <param name="weapon"> The weapon that should be reloaded. </param>
    /// <returns> true if it succeeds, false if it fails. </returns>
    public ReloadResult Reload(StorageContainer inventory, IGun weapon)
    {
      var countController = new InventoryCountController(inventory);

      if (weapon.ShotsRemaining == weapon.BaseStats.ClipSize)
        return ReloadResult.ClipIsAlreadyFilled;

      int initialCount = weapon.ShotsRemaining;

      while (weapon.ShotsRemaining < weapon.BaseStats.ClipSize)
      {
        // TODO update
        var ammoCursor = countController.GetAmmoCursor(GlobalResources.Ammos[0].AmmoType);
        var ammoStack = ammoCursor.Stack;

        if (ammoStack.IsEmpty)
          break;

        inventory.Decrement(ammoCursor);

        weapon.ShotsRemaining ++;
      }

      // we didn't add any
      if (weapon.ShotsRemaining == initialCount)
        return ReloadResult.OutOfAmmo;
      else if (weapon.ShotsRemaining == weapon.BaseStats.ClipSize)
        return ReloadResult.ClipHasBeenFilled;
      else
        return ReloadResult.AddedSomeAndNowOutOfAmmo;
    }

    /// <summary> All of the possible results of firing a weapon. </summary>
    public enum FireResult
    {
      /// <summary> The weapon needs to be reloaded. </summary>
      OutOfAmmo,

      /// <summary> Nothing was hit. </summary>
      Missed,

      /// <summary> Something was hit. </summary>
      Hit
    }

    /// <summary> All of the possible results of reloading a weapon. </summary>
    internal enum ReloadResult
    {
      /// <summary> The clip has already been filled. </summary>
      ClipIsAlreadyFilled,

      /// <summary> An enum constant representing the filled option. </summary>
      ClipHasBeenFilled,

      /// <summary> We added some ammo to the clip, but now we're out. </summary>
      AddedSomeAndNowOutOfAmmo,

      /// <summary> We're out of ammo for that weapon. </summary>
      OutOfAmmo
    }
  }
}