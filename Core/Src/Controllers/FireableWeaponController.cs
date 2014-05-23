using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using BringBackSociety.Services;
using log4net;

namespace BringBackSociety.Controllers
{
  /// <summary> Handles firing of projectile weapons. </summary>
  public class FireableWeaponController
  {
    /// <summary> Provides logging for the class. </summary>
    private static readonly ILog Log = LogManager.GetLogger(typeof(FireableWeaponController));

    private readonly IRaycastService _raycastService;
    private readonly IFirableWeaponView _weaponView;

    /// <summary> Constructor. </summary>
    public FireableWeaponController(IRaycastService raycastService, IFirableWeaponView weaponView)
    {
      _raycastService = raycastService;
      _weaponView = weaponView;
    }

    /// <summary> Have the actor fire their primary weapon. </summary>
    /// <param name="actor"> The actor who should fire their weapon. </param>
    public void FireWeapon(IActor actor)
    {
      var weaponSlot = actor.Inventory.GetStack(actor.EquippedWeapon);
      var weapon = weaponSlot.Model as IFireableWeaponModel;

      var ammoCursor = new InventoryCountController(actor.Inventory).GetAmmoCursor(weapon);
      var ammoStack = ammoCursor.Stack;

      // if they can't afford it, early exit
      if (ammoStack.IsEmpty || weapon == null)
        return;

      var hitObject = _raycastService.Raycast<IDestroyable>(actor, weapon.MaxDistance);
      if (hitObject != null)
      {
        Log.InfoFormat("Hit {0} with {1}", hitObject, weapon);
        Damage(hitObject, weapon.DamagePerShot);
        actor.Inventory.Decrement(ammoCursor);
      }

      _weaponView.FireWeapon(actor, weapon);
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
  }
}