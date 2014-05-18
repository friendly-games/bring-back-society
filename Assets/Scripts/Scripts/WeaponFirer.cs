using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behavior;
using BringBackSociety.Controllers;
using BringBackSociety.Items;
using Drawing;
using log4net;
using UnityEngine;

namespace Scripts
{
  public class WeaponFirer : MonoBehaviour, IGui
  {
    public Light LightSource;
    public AudioSource AudioSource;
    private Transform _parentTransform;

    /// <summary> Provides logging for the class. </summary>
    private static readonly ILog Log = LogManager.GetLogger(typeof(WeaponFirer));

    private int _curWeaponIndex;

    private PropertyDrawer _weaponDrawer;
    private StorageContainer _inventory;
    private StorageContainer.Cursor _currentWeapon;

    // Use this for initialization
    public void Start()
    {
      _parentTransform = transform.parent.transform;

      LightSource = GetComponentsInChildren<Light>().First();
      AudioSource = GetComponentsInChildren<AudioSource>().First();

      _weaponDrawer = new PropertyDrawer("Weapons", Screen.width - 120, 10);

      _inventory = new StorageContainer(10);
      GlobalResources.Instance.Weapons.Take(10)
                     .Select(w => new InventoryStack(w, 20))
                     .ToList()
                     .ForEach(itemStack => _inventory.AddToStorage(itemStack));

      SwitchWeapons(0);
    }

    public void Fire()
    {
      StartCoroutine(FireWeapon());
    }

    public void SwitchWeapons(int weapon)
    {
      if (weapon < 0 || weapon >= _inventory.Slots.Count)
        return;

      _curWeaponIndex = weapon;
      _currentWeapon = _inventory.GetCursor(_curWeaponIndex);
      var itemSlot = _inventory.Slots[weapon];

      Log.InfoFormat("Switched weapon to {0}", itemSlot);
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
        else
        {
          _weaponDrawer.AddItem(label + " - " + slot.Model.Name, slot.Quantity.ToString());
        }
      }

      _weaponDrawer.Draw();
    }

    private IEnumerator FireWeapon()
    {
      var itemQuanity = _inventory.GetStack(_currentWeapon);
      var weapon = itemQuanity.Model as IFireableWeaponModel;

      // if they can't afford it, early exit
      if (itemQuanity.IsEmpty || weapon == null)
        yield break;

      LightSource.enabled = true;
      AudioSource.Play();

      RaycastHit hitInfo;

      if (Physics.Raycast(new Ray(_parentTransform.position, _parentTransform.forward),
                          out hitInfo,
                          weapon.MaxDistance))
      {
        var killable = hitInfo.collider.gameObject.Get<IDestroyable>();

        if (killable != null)
        {
          Log.Info("Hit Enemy");
          Damage(killable, weapon.DamagePerShot);

          _inventory.Decrement(_currentWeapon);
        }
      }

      yield return new WaitForSeconds(0.05f);
      LightSource.enabled = false;
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