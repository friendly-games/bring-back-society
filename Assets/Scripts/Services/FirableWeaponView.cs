using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behavior;
using BringBackSociety.Items;
using BringBackSociety.Services;
using log4net;
using UnityEngine;

namespace Services
{
  /// <summary>  Unity implementation of the FirableWeaponView. </summary>
  internal class FirableWeaponView : IFirableWeaponView
  {
    private readonly MonoBehaviour _owner;
    private readonly Light _light;
    private readonly AudioSource _audio;

    public FirableWeaponView(MonoBehaviour owner)
    {
      var weaponGameObject = GameObject.Find("Weapon");

      _owner = owner;
      _light = weaponGameObject.GetComponentsInChildren<Light>().First();
      _audio = weaponGameObject.GetComponentsInChildren<AudioSource>().First();
    }

    void IFirableWeaponView.FireWeapon(IPlayer actor, IFireableWeaponModel weapon)
    {
      var actualWeapon = (Weapon) weapon;
      _owner.StartCoroutine(FireWeapon(actualWeapon));
    }

    private IEnumerator FireWeapon(Weapon actualWeapon)
    {
      var light = _light ?? actualWeapon.lightFlash;
      var audio = _audio ?? actualWeapon.audioSource;

      light.enabled = true;
      audio.Play();

      yield return new WaitForSeconds(0.05f);
      light.enabled = false;
    }
  }
}