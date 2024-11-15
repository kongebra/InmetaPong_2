using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    [Header("Ball Stats")]

    [SerializeField]
    private float _initialSpeed = 15f;
    private float _speed = 10f;
    [SerializeField]
    private float _maxSpeed = 64f;

    private bool _idleState = true;

    [Header("Game Events")]
    [SerializeField]
    private GameEvent _onBallHitPlayer;
    [SerializeField]
    private GameEvent _onBallHitDeathWall;

    [Header("Audio Settings")]
    [SerializeField, Range(0f, 1f)]
    private float _gameVolume = 1f;
    [SerializeField, Range(0f, 1f)]
    private float _idleGameVolume = 0.25f;

    private Vector2 _direction = Vector2.zero;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        Reset(true);
    }

    public void Reset(bool idleState = false)
    {
        _idleState = idleState;

        _audioSource.volume = _idleState ? _idleGameVolume : _gameVolume;

        _speed = _initialSpeed;
        transform.position = Vector3.zero;

        _direction = (-Vector2.one).normalized;
    }

    private void FixedUpdate()
    {
        transform.Translate(_direction * _speed * Time.fixedDeltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") &&
            (collision.gameObject.name == "Top" || collision.gameObject.name == "Bottom"))
        {
            Debug.Log("Prolonged collision with wall");
            _direction = Vector2.Reflect(_direction, collision.contacts[0].normal);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePaddleHit(collision);

            if (!_idleState)
            {
                _onBallHitPlayer?.Raise();
                _speed *= GameManager.Instance.SpeedMultiplier;

                _speed = Mathf.Min(_speed, _maxSpeed);

                Debug.Log($"Ball speed: {_speed}");
            }

            _audioSource.Play();

        }

        else if (collision.gameObject.CompareTag("Wall"))
        {
            // If it's not the right wall, bounce off normally
            _direction = Vector2.Reflect(_direction, collision.contacts[0].normal);

            _audioSource.Play();
        }

        else if (collision.gameObject.CompareTag("DeathWall"))
        {
            _onBallHitDeathWall?.Raise();
            _speed = 0f;

            // TODO: Any sound or visual effects here?
        }
    }

    private const float MAX_Y = 1.0f;
    private void HandlePaddleHit(Collision2D paddleCollision)
    {
        if (_idleState)
        {
            _direction.x = -_direction.x;

            return;
        }

        // Calculate where the ball hit the paddle (-1 = bottom, 1 = top)
        float hitFactor = (transform.position.y - paddleCollision.transform.position.y) / paddleCollision.collider.bounds.size.y;

        // Clamp the hitFactor to prevent excessive vertical movement
        hitFactor = Mathf.Clamp(hitFactor, -0.7071f, 0.7071f);  // This limits the vertical reflection to approximately 45 degrees

        // Adjust the y-direction based on where the ball hit the paddle
        _direction.y += hitFactor;

        var positive = _direction.y > 0;
        var negative = _direction.y < 0;

        if (positive && _direction.y > MAX_Y)
        {
            _direction.y = MAX_Y;
        }

        if (negative && _direction.y < -MAX_Y)
        {
            _direction.y = -MAX_Y;
        }

        // Normalize the _direction to ensure consistent speed
        _direction = _direction.normalized;

        // Since the ball hit the player, reverse the x _direction
        _direction.x = -_direction.x;
    }
}
