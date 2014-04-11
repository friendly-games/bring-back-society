using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
internal class CharacterMover : MonoBehaviour
{
  public float MaxSpeed = 100.0f;

  public float MaxRotation = 10f;

  [Range(0.0f, 1.0f)]
  public float FollowDrag = 0.2f;

  private Rigidbody _rigidbody;
  private Transform _transform;

  /// <summary> The target velocity of the item. </summary>
  public Vector3 TargetVelocity { get; set; }

  public Quaternion TargetRotation { get; set; }

  public void Start()
  {
    _rigidbody = GetComponent<Rigidbody>();
    _transform = GetComponent<Transform>();

    TargetRotation = _transform.rotation;
  }

  public void Update()
  {
    var targetVelocity = TargetVelocity;
    var currentVelocity = _rigidbody.velocity;
    targetVelocity.y = 0;
    currentVelocity.y = 0;

    var diffVelocity = (targetVelocity*MaxSpeed) - currentVelocity;
    _rigidbody.AddForce(diffVelocity, ForceMode.Impulse);

    _transform.rotation = Quaternion.Slerp(_transform.rotation,
                                           TargetRotation,
                                           Time.deltaTime*MaxRotation);
  }
}