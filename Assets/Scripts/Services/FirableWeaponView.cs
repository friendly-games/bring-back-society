using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using BringBackSociety.Services;
using UnityEngine;

namespace Services
{
  /// <summary>  Unity implementation of the FirableWeaponView. </summary>
  internal class FirableWeaponView : IFirableWeaponView
  {
    private readonly MonoBehaviour _owner;
    private readonly Light _light;
    private readonly AudioSource _audio;

    public FirableWeaponView(MonoBehaviour owner, Light light, AudioSource audio)
    {
      _owner = owner;
      _light = light;
      _audio = audio;
    }

    void IFirableWeaponView.FireWeapon(IActor actor, IFireableWeaponModel weapon)
    {
      //var actualWeapon = (Weapon) weapon;
      _owner.StartCoroutine(FireWeapon());
    }

    private IEnumerator FireWeapon()
    {
      _light.enabled = true;
      _audio.Play();

      yield return new WaitForSeconds(0.05f);
      _light.enabled = false;
    }
  }
}