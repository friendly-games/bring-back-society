using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using BringBackSociety.Items.Weapons;
using UnityEngine;

namespace Models
{
  /// <summary> Represents a weapon which can be fired. </summary>
  public class FireableWeaponModel : ExtendedBehaviour, IModelProvider<IFireableWeaponModel>
  {
    private Implementation _implementation;

    /// <inheritdoc />
    IFireableWeaponModel IModelProvider<IFireableWeaponModel>.ModelImplementation
    {
      get { return _implementation; }
    }

    public void Awake()
    {
      _implementation = new Implementation(gameObject, this);
    }

    /// <summary> The actual implementation of the given model. </summary>
    private class Implementation : BaseModel<FireableWeaponModel, IFireableWeaponModel>,
                                   IFireableWeaponModel
    {
      private readonly Light _light;
      private readonly AudioSource _audio;

      private FireableWeaponState _state;

      public Implementation(GameObject gameObject, FireableWeaponModel behavior)
        : base(gameObject, behavior)
      {
        _light = gameObject.GetComponentsInChildren<Light>().First();
        _audio = gameObject.GetComponentsInChildren<AudioSource>().First();

        _state = FireableWeaponState.None;
      }

      /// <inheritdoc />
      public FireableWeapon FireableWeapon { get; set; }

      /// <inheritdoc />
      public bool CanEnterState(FireableWeaponState state)
      {
        return _state == FireableWeaponState.None;
      }

      /// <inheritdoc />
      public void TransitionToState(FireableWeaponState state)
      {
        Behavoir.StartCoroutine(FireWeapon());
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
    }
  }
}