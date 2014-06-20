using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using BringBackSociety.Items.Weapons;
using BringBackSociety.Services;
using log4net;

namespace BringBackSociety.Controllers
{
  /// <summary> Controls a single player in the game. </summary>
  internal class PlayerController
  {
    /// <summary> Provides logging for the class. </summary>
    private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerController));

    private readonly IPlayer _player;
    private readonly FireableWeaponController _weaponController;

    /// <summary> Constructor. </summary>
    public PlayerController(IPlayer player, FireableWeaponController weaponController)
    {
      if (player == null)
        throw new ArgumentNullException("player");

      _player = player;
      _weaponController = weaponController;
    }

    /// <summary> Use the current item </summary>
    /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
    ///  the required range. </exception>
    public void UseItem()
    {
      var weaponSlot = _player.Inventory.GetStack(_player.EquippedItem);
      var weapon = weaponSlot.Model as FireableWeapon;

      if (weapon != null)
      {
        UseWeapon(weapon, _player.EquippedItemHost.CurrentModel);
      }
    }

    private void UseWeapon(FireableWeapon weapon, IFireableWeaponModel model)
    {
      _player.Inventory.ForceSnapshot();

      var result = _weaponController.FireWeapon(_player, weapon);

      bool wasFired = false;

      switch (result)
      {
        case FireableWeaponController.FireResult.OutOfAmmo:
          Log.Info("Out of Ammo");
          _weaponController.Reload(_player.Inventory, weapon);
          Log.Info("Reloaded");
          break;

        case FireableWeaponController.FireResult.Missed:
          Log.Info("Fired, but missed");
          wasFired = true;
          break;

        case FireableWeaponController.FireResult.Hit:
          Log.Info("Fired and hit");
          wasFired = true;
          break;

        default:
          throw new ArgumentOutOfRangeException();
      }

      if (wasFired && model != null)
      {
        model.TransitionToState(FireableWeaponState.Fired);
      }
    }
  }
}