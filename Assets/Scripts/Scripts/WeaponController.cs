using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Behavior;
using log4net;
using UnityEngine;

namespace Scripts
{
  public class WeaponController : MonoBehaviour
  {
    public Light LightSource;
    public AudioSource AudioSource;
    private Transform _parentTransform;

    public Weapon[] Weapons;

    private readonly ILog _log = LogManager.GetLogger(typeof (WeaponController));

    // Use this for initialization
    public void Start()
    {
      _parentTransform = transform.parent.transform;

      LightSource = GetComponentsInChildren<Light>().First();
      AudioSource = GetComponentsInChildren<AudioSource>().First();
    }

    public void Fire()
    {
      StartCoroutine(FireWeapon());
    }

    private IEnumerator FireWeapon()
    {
      LightSource.enabled = true;
      AudioSource.Play();

      RaycastHit hitInfo;

      bool didHit = false;

      var weapon = Weapons.First();

      if (Physics.Raycast(new Ray(_parentTransform.position, _parentTransform.forward),
                          out hitInfo,
                          weapon.MaxDistance))
      {
        var killable = hitInfo.collider.gameObject.Get<IKillable>();

        if (killable != null)
        {
          _log.Info("Hit Enemy");
          killable.Damage(weapon.DamagePerShot);
          didHit = true;
        }
      }

      if (!didHit)
      {
        _log.Info("Missed enemies");
      }

      yield return new WaitForSeconds(0.05f);
      LightSource.enabled = false;
    }
  }
}