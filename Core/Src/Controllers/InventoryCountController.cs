using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using BringBackSociety.Items.Weapons;

namespace BringBackSociety.Controllers
{
  /// <summary> Retrieves the counts of items in inventory. </summary>
  public class InventoryCountController
  {
    private readonly StorageContainer _container;

    /// <summary> Constructor. </summary>
    /// <param name="container"> The container whose contents should be queried. </param>
    public InventoryCountController(StorageContainer container)
    {
      if (container == null)
        throw new ArgumentNullException("container");

      _container = container;
    }

    /// <summary> Gets the quantity to display for the given stack. </summary>
    /// <param name="stack"> The stack for which the count should be retrieved. </param>
    /// <returns> The display count for the stack. </returns>
    public int GetDisplayCount(InventoryStack stack)
    {
      var model = stack.Model;

      var weaponModel = model as IFireableWeaponModel;
      if (weaponModel != null)
      {
        return GetDisplayCount(weaponModel);
      }
      else
      {
        return stack.Quantity;
      }
    }

    /// <summary> Sums the quantity of items in the inventory for the given model </summary>
    private int GetCountOfAllModel(IItemModel model)
    {
      int currentCount = 0;

      foreach (InventoryStack slot in _container)
      {
        if (slot.Model == model)
        {
          currentCount += slot.Quantity;
        }
      }

      return currentCount;
    }

    /// <summary> Gets the cursor to the ammo that should be used when firing the given weapon. </summary>
    /// <param name="ammoType"> The type of ammo slot that should be found. </param>
    /// <returns> A cursor pointing to the slot that should be decremented when the weapon is fired. </returns>
    /// <remarks>TODO this method should be moved elsewhere. </remarks>
    public StorageContainer.Cursor GetAmmoCursor(AmmoType ammoType)
    {
      var lowest = StorageContainer.Cursor.Empty;

      foreach (var cursor in _container)
      {
        var slot = cursor.Stack;
        var ammoModel = slot.Model as IAmmoModel;

        if (ammoModel != null && ammoType == ammoModel.AmmoType)
        {
          if (lowest.Stack.IsEmpty || slot.Quantity < lowest.Stack.Quantity)
          {
            lowest = cursor;
          }
        }
      }

      return lowest;
    }

    /// <summary> Gets the ammo count for a specified weapon. </summary>
    private int GetDisplayCount(IFireableWeaponModel weapon)
    {
      int currentCount = 0;

      foreach (InventoryStack slot in _container)
      {
        var ammoModel = slot.Model as IAmmoModel;

        if (ammoModel != null && weapon.AmmoType == ammoModel.AmmoType)
        {
          currentCount += slot.Quantity;
        }
      }

      return currentCount;
    }
  }
}