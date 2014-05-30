using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Items;
using UnityEngine;

namespace Models
{
  /// <summary> Represents a weapon which can be fired. </summary>
  public class FireableWeaponModel : ExtendedBehaviour, IStart, IModelProvider<IFireableWeaponView>
  {
    private Implementation _implementation;

    /// <inheritdoc />
    IFireableWeaponView IModelProvider<IFireableWeaponView>.ModelImplementation
    {
      get { return _implementation; }
    }

    public void Start()
    {
      _implementation = new Implementation(gameObject, this);
      Logging.Log.Info("Done implementing");
    }

    /// <summary> The actual implementation of the given model. </summary>
    private class Implementation : BaseModel<FireableWeaponModel, IFireableWeaponView>,
                                   IFireableWeaponView
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