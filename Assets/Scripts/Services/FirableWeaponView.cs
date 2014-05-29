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
  internal class FirableWeaponView : IFireableWeaponView
  {
    private readonly MonoBehaviour _owner;
    private readonly Light _light;
    private readonly AudioSource _audio;

    private FireableWeaponState _state;

    public FirableWeaponView(MonoBehaviour owner)
    {
      var weaponGameObject = GameObject.Find("Weapon");

      _owner = owner;
      _light = weaponGameObject.GetComponentsInChildren<Light>().First();
      _audio = weaponGameObject.GetComponentsInChildren<AudioSource>().First();

      _state = FireableWeaponState.None;
    }

    public bool CanEnterState(FireableWeaponState state)
    {
      return _state == FireableWeaponState.None;
    }

    public void TransitionToState(FireableWeaponState state)
    {
      _owner.StartCoroutine(FireWeapon());
    }

    public void AttachTo(IPlayer player)
    {
      throw new NotImplementedException();
    }

    private IEnumerator FireWeapon()
    {
      _state = FireableWeaponState.Fired;

      var light = _light;
      var audio = _audio;

      light.enabled = true;
      audio.Play();

      yield return new WaitForSeconds(0.05f);
      light.enabled = false;

      _state = FireableWeaponState.None;
    }

    public IFireableWeaponView Copy()
    {
      return null;
    }

    object ICopyable.Copy()
    {
      return Copy();
    }
  }
}