using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;

    private PlayerInputActions _inputActions;

    private Vector2 _movement;

    public float bounds = 5f;
    public float padelLength = 2f;

    private float yCalculateBound { get { return bounds - (padelLength / 2f); } }

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
        _inputActions?.Disable();
    }

    public void EnableControls()
    {
        _inputActions?.Enable();
    }

}
