using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
  [RequireComponent(typeof(Rigidbody))]
  internal class CharacterMover : MonoBehaviour
  {
    public float MaxSpeed = 100.0f;

    public float MaxRotation = 10f;

    public float JumpSpeed = 1.0f;

    [Range(0.0f, 1.0f)]
    public float FollowDrag = 0.2f;

    private Rigidbody _rigidbody;
    private Transform _transform;
    private Rigidbody _ground;
    private DateTime _lastJumpTime;

    /// <summary> The target velocity of the item. </summary>
    public Vector3 TargetVelocity { get; set; }

    public Quaternion TargetRotation { get; set; }

    /// <summary> True if the character should be jumping if possible.  </summary>
    public bool AttemptJump { get; set; }

    public void Start()
    {
      _rigidbody = GetComponent<Rigidbody>();
      _transform = GetComponent<Transform>();
      _ground = GameObject.Find("Ground").rigidbody;

      TargetRotation = _transform.rotation;
    }

    public void Update()
    {
      var targetVelocity = TargetVelocity;
      var currentVelocity = _rigidbody.velocity;
      targetVelocity.y = 0;
      currentVelocity.y = 0;

      targetVelocity *= MaxSpeed;

      var diffVelocity = (targetVelocity - currentVelocity) * Time.deltaTime * 300;

      if (AttemptJump && DateTime.Now - _lastJumpTime > TimeSpan.FromMilliseconds(100))
      {
        // TODO replace constants
        // var ray = new Ray(_transform.position, Vector3.down);
        //RaycastHit hitInfo;
        //if (_ground.collider.Raycast(ray, out hitInfo, 1.05f))
        {
          diffVelocity.y = JumpSpeed * 30;
          _lastJumpTime = DateTime.Now;
        }
      }

      _rigidbody.AddForce(diffVelocity, ForceMode.Acceleration);

      _transform.rotation = Quaternion.Slerp(_transform.rotation,
                                             TargetRotation,
                                             Time.deltaTime * MaxRotation);
    }
  }
}