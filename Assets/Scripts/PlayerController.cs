using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Padel Stats")]
    [SerializeField]
    private float _speed = 5f;

    [Header("References")]
    [SerializeField]
    private Ball _ball;

    private bool _idleState = true;

    private PlayerInputActions _inputActions;

    private Vector2 _movement;

    [Header("Padel Bounds")]
    [SerializeField]
    private float _bounds = 11f;
    [SerializeField]
    private float _padelLength = 4f;

    private float yCalculateBound { get { return _bounds - (_padelLength / 2f); } }

    private bool _inputEnabled = false;
    private void Awake()
    {
        _inputActions = new PlayerInputActions();

        _inputActions.Player.Move.performed += ctx =>
        {
            if (_inputEnabled)
            {
                _movement = ctx.ReadValue<Vector2>();
            }
        };
        _inputActions.Player.Move.canceled += ctx =>
        {
            if (_inputEnabled)
            {
                _movement = Vector2.zero;
            }
        };
    }

    private void FixedUpdate()
    {
        if (_idleState)
        {
            if (_ball != null)
            {
                var ballPos = _ball.transform.position;
                var pos = transform.position;


                // lerp Y position to ball position
                pos.y = Mathf.Lerp(pos.y, ballPos.y, Time.fixedDeltaTime * _speed);

                // clamp Y position
                pos.y = Mathf.Clamp(pos.y, -yCalculateBound, yCalculateBound);

                transform.position = pos;
            }

            return;
        }

        var movement = new Vector3(0, _movement.y, 0);
        var nextPosition = transform.position + movement.normalized * _speed * Time.fixedDeltaTime;

        nextPosition.y = Mathf.Clamp(nextPosition.y, -yCalculateBound, yCalculateBound);

        transform.position = nextPosition;
    }

    public void ResetPosition()
    {
        var pos = transform.position;
        pos.y = 0;

        transform.position = pos;
    }

    private void OnEnable()
    {
        _inputActions?.Enable();
    }

    private void OnDisable()
    {
        _inputActions?.Disable();
    }

    public void DisableControls()
    {
        _inputEnabled = false;
    }

    public void EnableControls()
    {
        ResetPosition();
        _inputEnabled = true;
        _idleState = false;
    }

    public void HandleGameOver()
    {
        _movement = Vector2.zero;
    }

    public void TurnOnIdleState()
    {
        ResetPosition();
        _idleState = true;
    }
}
