using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControllerV2 : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private const string DEATH_WALL_TAG = "Death Wall";

    [Header("Ball Settings")]
    [SerializeField]
    private float _initialSpeed = 10f;
    [SerializeField]
    private float _speedCoefficient = 1.1f;
    private float _currentSpeed = 0f;

    [Header("Game Events")]
    [SerializeField]
    private GameEvent _onBallHitPlayer;
    [SerializeField]
    private GameEvent _onBallHitDeathWall;

    private Rigidbody2D _rb;
    private Vector2 _direction;
    private BoxCollider2D _bounds;

    private float RADIUS => 0.25f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        _currentSpeed = _initialSpeed;
    }

    private void FixedUpdate()
    {
        var step = _rb.position + _direction * _currentSpeed * Time.fixedDeltaTime;

        if (step.y + RADIUS > _bounds.bounds.max.y || step.y - RADIUS < _bounds.bounds.min.y)
        {
            _direction.y *= -1f;

            // recalculate step
            step = _rb.position + _direction * _currentSpeed * Time.fixedDeltaTime;
        }

        if (step.x + RADIUS > _bounds.bounds.max.x)
        {
            _direction.x *= -1f;

            // recalculate step
            step = _rb.position + _direction * _currentSpeed * Time.fixedDeltaTime;
        }

        _rb.MovePosition(step);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(PLAYER_TAG))
        {
            var hitFactor = (transform.position.y - other.transform.position.y) / other.bounds.size.y;

            // 1f = right direction
            _direction = new Vector2(1f, hitFactor).normalized;

            _currentSpeed *= _speedCoefficient;

            _onBallHitPlayer?.Raise();
        }

        if (other.CompareTag(DEATH_WALL_TAG))
        {
            _onBallHitDeathWall?.Raise();

            // TODO: Simple particles for showing the ball explodes?

            Destroy(gameObject);
        }
    }

    internal void SetBounds(BoxCollider2D bounds)
    {
        _bounds = bounds;
    }

    internal void Init()
    {
        _currentSpeed = _initialSpeed;
        _direction = new Vector2(1f, Random.Range(-1f, 1f)).normalized;
    }
}
