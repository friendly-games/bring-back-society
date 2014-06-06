using System;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary> Constructor. </summary>
    public FireableWeaponController(IRaycastService raycastService)
    {
      _raycastService = raycastService;
    }

    /// <summary> Have the actor fire their primary weapon. </summary>
    /// <param name="player"> The actor who should fire their weapon. </param>
    /// <param name="weapon"> The weapon that should be fired. </param>
    /// <returns> The result of firing the weapon </returns>
    public FireResult FireWeapon(IPlayer player, FireableWeapon weapon)
    {
      if (weapon.ShotsRemaining == 0)
        return FireResult.OutOfAmmo;

      var result = FireResult.Missed;

      var stats = weapon.Stats;

      var rand = new System.Random();

      var right = player.Transform.right;
      var up = player.Transform.up;

      for (int i = 0; i < stats.NumberOfPellets; i++)
      {
        var ray = player.Transform.ToRay();

        if (stats.Spread > 0)
        {
          var offset = right * rand.Next(-20, 20) / 100.0f
                       + up * rand.Next(-20, 20) / 100.0f;

          ray.direction += offset;
        }

        float distance;
        var hitObject = _raycastService.Raycast<IDestroyable>(ray, stats.MaxDistance, out distance);

        Debugging.Drawing.Draw(ray, distance);

        if (hitObject != null)
        {
          Log.InfoFormat("Hit {0} with {1}", hitObject, weapon);
          Damage(hitObject, weapon.Stats.DamagePerShot);

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
    public ReloadResult Reload(StorageContainer inventory, FireableWeapon weapon)
    {
      var countController = new InventoryCountController(inventory);

      if (weapon.ShotsRemaining == weapon.Stats.ClipSize)
        return ReloadResult.ClipIsAlreadyFilled;

      int initialCount = weapon.ShotsRemaining;

      while (weapon.ShotsRemaining < weapon.Stats.ClipSize)
      {
        var ammoCursor = countController.GetAmmoCursor(weapon.Stats.AmmoType);
        var ammoStack = ammoCursor.Stack;

        if (ammoStack.IsEmpty)
          break;

        inventory.Decrement(ammoCursor);

        weapon.ShotsRemaining ++;
      }

      // we didn't add any
      if (weapon.ShotsRemaining == initialCount)
        return ReloadResult.OutOfAmmo;
      else if (weapon.ShotsRemaining == weapon.Stats.ClipSize)
        return ReloadResult.ClipHasBeenFilled;
      else
        return ReloadResult.AddedSomeAndNowOutOfAmmo;
    }

    /// <summary> Perform damage on the killable object </summary>
    /// <param name="destroyable"> The killable to act on. </param>
    /// <param name="damageAmount">The amount of damage to perform. </param>
    private void Damage(IDestroyable destroyable, int damageAmount)
    {
      destroyable.Health -= (int) (damageAmount / destroyable.Resistance.BulletResistance);
      Log.InfoFormat("Damaged {0}. Health remaining: {1}", destroyable, destroyable.Health);

      if (destroyable.Health <= 0)
      {
        destroyable.Destroy();
      }
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