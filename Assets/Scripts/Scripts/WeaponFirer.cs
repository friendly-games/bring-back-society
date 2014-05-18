using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behavior;
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
    private static readonly ILog Log = LogManager.GetLogger(typeof (WeaponFirer));

    private int _curWeaponIndex;

    private PropertyDrawer _weaponDrawer;
    private IFireableWeaponModel[] _fireableWeaponsModel;
    private int[] _counts;

    // Use this for initialization
    public void Start()
    {
      _parentTransform = transform.parent.transform;

      LightSource = GetComponentsInChildren<Light>().First();
      AudioSource = GetComponentsInChildren<AudioSource>().First();

      _weaponDrawer = new PropertyDrawer("Weapons", Screen.width - 120, 10);

      _fireableWeaponsModel = GlobalResources.Instance.Weapons;
      _counts = Enumerable.Repeat(20, _fireableWeaponsModel.Length).ToArray();

      SwitchWeapons(0);
    }

    public void Fire()
    {
      StartCoroutine(FireWeapon());
    }

    public void SwitchWeapons(int weapon)
    {
      if (_curWeaponIndex >= 0 && _curWeaponIndex < _fireableWeaponsModel.Length)
      {
        _curWeaponIndex = weapon;
        Log.InfoFormat("Switched weapon to {0}", _fireableWeaponsModel[_curWeaponIndex].Name);
      }
    }

    public void OnGUI()
    {
      _weaponDrawer.Start();

      for (var i = 0; i < _fireableWeaponsModel.Length; i++)
      {
        _weaponDrawer.AddItem(_fireableWeaponsModel[i].Name, _counts[i].ToString());
      }
      _weaponDrawer.Draw();
    }

    private IEnumerator FireWeapon()
    {
      var curWeapon = _fireableWeaponsModel[_curWeaponIndex];

      // if they can't afford it, early exit
      if (_counts[_curWeaponIndex] <= 0)
        yield break;

      LightSource.enabled = true;
      AudioSource.Play();

      RaycastHit hitInfo;

      if (Physics.Raycast(new Ray(_parentTransform.position, _parentTransform.forward),
                          out hitInfo,
                          curWeapon.MaxDistance))
      {
        var killable = hitInfo.collider.gameObject.Get<IDestroyable>();

        if (killable != null)
        {
          Log.Info("Hit Enemy");
          Damage(killable, curWeapon.DamagePerShot);
          _counts[_curWeaponIndex]--;
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