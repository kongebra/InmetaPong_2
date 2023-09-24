using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PadelController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    private PlayerInputActions _inputActions;

    private Vector2 _movement;

    private const float Y_BOUND = 4.5f; // 16x9 res, 9 / 2
    private const float PADEL_LENGTH = 1.5f; // Scale.y / 2f

    private float Y_CALCULATED_BOUND { get { return Y_BOUND - PADEL_LENGTH; } }

    private void Awake()
    {
        _inputActions = new PlayerInputActions();

        _inputActions.Player.Move.performed += ctx => _movement = ctx.ReadValue<Vector2>();

        _inputActions.Player.Move.canceled += ctx => _movement = Vector2.zero;
    }

    private void FixedUpdate()
    {
        var movement = new Vector3(0, _movement.y, 0);
        var nextPosition = transform.position + movement.normalized * _speed * Time.fixedDeltaTime;

        nextPosition.y = Mathf.Clamp(nextPosition.y, -Y_CALCULATED_BOUND, Y_CALCULATED_BOUND);

        transform.position = nextPosition;
    }

    private void OnEnable()
    {
        _inputActions?.Enable();
    }

    private void OnDisable()
    {
        _inputActions?.Disable();
    }
}
