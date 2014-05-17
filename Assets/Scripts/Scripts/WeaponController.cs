using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behavior;
using Drawing;
using log4net;
using UnityEngine;

namespace Scripts
{
  public class WeaponController : MonoBehaviour
  {
    public Light LightSource;
    public AudioSource AudioSource;
    private Transform _parentTransform;

    private readonly ILog _log = LogManager.GetLogger(typeof (WeaponController));
    private int _curWeaponIndex;

    private PropertyDrawer _weaponDrawer;
    private Weapon[] _weapons;
    private int[] _counts;

    // Use this for initialization
    public void Start()
    {
      _parentTransform = transform.parent.transform;

      LightSource = GetComponentsInChildren<Light>().First();
      AudioSource = GetComponentsInChildren<AudioSource>().First();

      _weaponDrawer = new PropertyDrawer("Weapons", Screen.width - 120, 10);

      _weapons = GlobalResources.Instance.Weapons;
      _counts = Enumerable.Repeat(20, _weapons.Length).ToArray();

      SwitchWeapons(0);
    }

    public void Fire()
    {
      StartCoroutine(FireWeapon());
    }

    public void SwitchWeapons(int weapon)
    {
      if (_curWeaponIndex >= 0 && _curWeaponIndex < _weapons.Length)
      {
        _curWeaponIndex = weapon;
        _log.InfoFormat("Switched weapon to {0}", _weapons[_curWeaponIndex].Name);
      }
    }

    public void OnGUI()
    {
      _weaponDrawer.Start();

      for (var i = 0; i < _weapons.Length; i++)
      {
        _weaponDrawer.AddItem(_weapons[i].Name, _counts[i].ToString());
      }
      _weaponDrawer.Draw();
    }

    private IEnumerator FireWeapon()
    {
      var curWeapon = _weapons[_curWeaponIndex];

      // if they can't afford it, early exit
      if (_counts[_curWeaponIndex] <= 0)
        yield break;

      LightSource.enabled = true;
      AudioSource.Play();

      RaycastHit hitInfo;

      bool didHit = false;

      if (Physics.Raycast(new Ray(_parentTransform.position, _parentTransform.forward),
                          out hitInfo,
                          curWeapon.MaxDistance))
      {
        var killable = hitInfo.collider.gameObject.Get<IDestroyable>();

        if (killable != null)
        {
          _log.Info("Hit Enemy");
          killable.Damage(curWeapon.DamagePerShot);
          didHit = true;
          _counts[_curWeaponIndex]--;
        }
      }

      yield return new WaitForSeconds(0.05f);
      LightSource.enabled = false;
    }
  }
}