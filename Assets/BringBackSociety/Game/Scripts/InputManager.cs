﻿using System;
using System.Collections.Generic;
using System.Linq;
using BringBackSociety.Game.Input;
using log4net;
using UnityEngine;

namespace Scripts
{
  /// <summary> Handles the input for the player. </summary>
  public class InputManager : MonoBehaviour
  {
    /// <summary> Provides logging for the class. </summary>
    private static readonly ILog Log = LogManager.GetLogger(typeof(InputManager));

    /// <summary> The object that should be moved using the input below. </summary>
    public GameObject Target;

    /// <summary> The mover. </summary>
    private CharacterMover _mover;

    public float DesiredSpeed = 1.0f;
    private Collider _groundCollider;
    private Bootstrap _weapon;
    private GameObject _camera;
    private Vector3 _playerPositionOffset;
    private GameObject _player;
    private float _playerY;

    private TargetLocator _locator;
    private Vector2 _lastMousePosition;

    public void Start()
    {
      _camera = GameObject.Find("Camera");

      _player = GameObject.Find("Player");
      _mover = _player.GetComponent<CharacterMover>();

      _groundCollider = GameObject.Find("Ground").collider;
      _weapon = GameObject.Find("Global").GetComponent<Bootstrap>();

      _playerY = _player.transform.position.y;
      _playerPositionOffset = _camera.transform.position - _player.transform.position;

      _locator = new TargetLocator(Screen.width, Screen.height, 0, 300);
    }

    public void Update()
    {
      if (Target == null)
        return;

      Vector3 targetVelocity = new Vector3();

      Vector3 forward = Vector3.forward;
      //Vector3 forward = Vector3.forward;
      Vector3 backward = Quaternion.AngleAxis(180, Vector3.up) * forward;
      Vector3 left = Quaternion.AngleAxis(-90, Vector3.up) * forward;
      Vector3 right = Quaternion.AngleAxis(90, Vector3.up) * forward;

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

      _mover.AttemptJump = Input.GetKey(KeyCode.Space);

      if (Input.GetKey(KeyCode.D))
      {
        targetVelocity += right;
      }

      if (Input.GetMouseButtonDown(0))
      {
        _weapon.Fire();
      }

      if (Input.GetKey(KeyCode.Alpha1))
      {
        _weapon.SwitchSlots(0);
      }

      if (Input.GetKey(KeyCode.Alpha2))
      {
        _weapon.SwitchSlots(1);
      }

      if (Input.GetKey(KeyCode.Alpha3))
      {
        _weapon.SwitchSlots(2);
      }

      targetVelocity.Normalize();

      _mover.TargetVelocity = targetVelocity;

      var diff = UpdateAndDiff(Input.mousePosition);
      _locator.Update(diff);

      var targetRotation = _locator.Direction;

      targetRotation.x = 0;
      targetRotation.z = 0;

      _mover.TargetRotation = targetRotation;
      _mover.TargetStrength = _locator.Strength * 100;

      Debug.DrawLine(new Vector3(0, 3, 0),
                     new Vector3(
                       _locator.EuelerDirection.x * _locator.Strength,
                       3,
                       _locator.EuelerDirection.y * _locator.Strength));

      var position = _player.transform.position;
      position.y = _playerY;

      _camera.transform.position = position + _playerPositionOffset;

      Screen.showCursor = false;
    }

    /// <summary> The last position calculated from all of the previous diffs. </summary>
    /// <remarks> Used to calculate newer diffs. </remarks>
    private Vector2 _lastRelativePosition;

    private Vector2 UpdateAndDiff(Vector2 newPosition)
    {
      var diff = newPosition - _lastRelativePosition;
      _lastRelativePosition = newPosition;
      return diff;
    }
  }
}