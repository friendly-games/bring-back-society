using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BringBackSociety.Items;

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

    public int GetDisplayCount(IItemModel model)
    {
      var weaponModel = model as IFireableWeaponModel;
      if (weaponModel != null)
      {
        return GetDisplayCount(weaponModel);
      }
      else
      {
        return GetGenericCount(model);
      }
    }

    private int GetGenericCount(IItemModel model)
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

    public StorageContainer.Cursor GetAmmoCursor(IFireableWeaponModel weapon)
    {
      foreach (var cursor in _container)
      {
        var slot = cursor.Stack;
        var ammoModel = slot.Model as IAmmoModel;

        if (ammoModel != null && weapon.AmmoType == ammoModel.AmmoType)
        {
          return cursor;
        }
      }

      return StorageContainer.Cursor.Empty;
    }

    /// <summary> Gets the ammo count for a specified weapon. </summary>
    /// <param name="weapon"> The weapon for which the ammo count should be displayed. </param>
    /// <returns> The display count for the weapon. </returns>
    public int GetDisplayCount(IFireableWeaponModel weapon)
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