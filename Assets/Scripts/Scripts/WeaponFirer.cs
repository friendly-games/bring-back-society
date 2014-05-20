using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behavior;
using BringBackSociety.Controllers;
using BringBackSociety.Items;
using Drawing;
using log4net;
using Services;
using UnityEngine;

namespace Scripts
{
  public class WeaponFirer : MonoBehaviour, IGui
  {
    /// <summary> Provides logging for the class. </summary>
    private static readonly ILog Log = LogManager.GetLogger(typeof(WeaponFirer));

    private int _curWeaponIndex;

    private PropertyDrawer _weaponDrawer;
    private FirableWeaponView _weaponView;
    private FireableWeaponController _firableWeaponController;
    private Player _player;
    private StorageContainer _inventory;

    // Use this for initialization
    public void Start()
    {
      _player = new Player(GameObject.Find("Player"));

      var weaponGameObject = GameObject.Find("Weapon");

      var weaponView = new FirableWeaponView(
        this,
        weaponGameObject.GetComponentsInChildren<Light>().First(),
        weaponGameObject.GetComponentsInChildren<AudioSource>().First()
        );

      _firableWeaponController = new FireableWeaponController(new RaycastService(), weaponView);

      _weaponDrawer = new PropertyDrawer("Weapons", Screen.width - 120, 10);

      GlobalResources.Instance.Weapons.Take(10)
                     .Select(w => new InventoryStack(w, 20))
                     .ToList()
                     .ForEach(itemStack => _player.Inventory.AddToStorage(itemStack));

      _inventory = _player.Inventory;

      SwitchWeapons(0);
    }

    public void Fire()
    {
      _firableWeaponController.FireWeapon(_player);
    }

    public void SwitchWeapons(int weapon)
    {
      if (weapon < 0 || weapon >= _inventory.Slots.Count)
        return;

      _curWeaponIndex = weapon;
      _player.EquippedWeapon = _inventory.GetCursor(_curWeaponIndex);

      Log.InfoFormat("Switched weapon to {0}", _inventory.Slots[weapon]);
    }

    public void OnGUI()
    {
      _weaponDrawer.Start();

      for (var i = 0; i < _inventory.Slots.Count; i++)
      {
        var slot = _inventory.Slots[i];

        string label = (i + 1).ToString();

        if (slot.IsEmpty)
        {
          _weaponDrawer.AddItem(label + " - ", " - ");
        }
        else if (i == _player.EquippedWeapon.SlotNumber)
        {
          _weaponDrawer.AddItem("*" + " " + slot.Model.Name, slot.Quantity.ToString());
        }
        else
        {
          _weaponDrawer.AddItem(label + " " + slot.Model.Name, slot.Quantity.ToString());
        }
      }

      _weaponDrawer.Draw();
    }
  }
}