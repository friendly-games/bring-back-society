using System.Linq;
using Behavior;
using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
  public Light LightSource;
  public AudioSource AudioSource;
  private Transform _parentTransform;

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
    if (Physics.Raycast(new Ray(_parentTransform.position, _parentTransform.forward), out hitInfo, 100))
    {
      var otherObject = hitInfo.collider.gameObject;
      var killable = otherObject.Get<IKillable>();
      if (killable != null)
      {
        Debug.Log("Hit Enemy");
        killable.Damage(50);
      }
      else
      {
        Debug.Log("Enemy missed");
      }
    }

    yield return new WaitForSeconds(0.05f);
    LightSource.enabled = false;
  }
}