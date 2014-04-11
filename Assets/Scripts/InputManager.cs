using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
  /// <summary> The object that should be moved using the input below. </summary>
  public GameObject Target;

  /// <summary> The mover. </summary>
  private CharacterMover _mover;

  public float DesiredSpeed = 1.0f;
  private Collider _groundCollider;
  private WeaponController _weapon;

  public void Start()
  {
    GameObject playerObject = GameObject.Find("Player");
    _mover = playerObject.GetComponent<CharacterMover>();

    _groundCollider = GameObject.Find("Ground").collider;
    _weapon = GameObject.Find("Weapon").GetComponent<WeaponController>();
  }

  public void Update()
  {
    if (Target == null)
      return;

    Vector3 targetVelocity = new Vector3();

    Vector3 forward = Vector3.forward;
    //Vector3 forward = Vector3.forward;
    Vector3 backward = Quaternion.AngleAxis(180, Vector3.up)*forward;
    Vector3 left = Quaternion.AngleAxis(-90, Vector3.up)*forward;
    Vector3 right = Quaternion.AngleAxis(90, Vector3.up)*forward;

    if (Input.GetKey(KeyCode.W))
    {
      targetVelocity += forward;
    }

    if (Input.GetKey(KeyCode.S))
    {
      targetVelocity += backward;
    }

    if (Input.GetKey(KeyCode.A))
    {
      targetVelocity += left;
    }

    if (Input.GetKey(KeyCode.D))
    {
      targetVelocity += right;
    }

    if (Input.GetMouseButtonDown(0))
    {
      _weapon.Fire();
    }

    targetVelocity.Normalize();

    _mover.TargetVelocity = targetVelocity;

    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    RaycastHit hitInfo;
    if (_groundCollider.Raycast(ray, out hitInfo, 10000f))
    {
      var diff = hitInfo.point - _mover.transform.position;
      diff.y = 0;

      var targetRotation = Quaternion.FromToRotation(Vector3.forward, diff).eulerAngles;

      targetRotation.x = 0;
      targetRotation.z = 0;

      _mover.TargetRotation = Quaternion.Euler(targetRotation);
    }
  }
}