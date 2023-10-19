using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Ball Stats")]
    [SerializeField]
    private float _speed = 10f;

    [Header("Game Events")]
    [SerializeField]
    private GameEvent _onBallHitPlayer;

    private Vector2 _direction = Vector2.zero;

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        transform.position = Vector3.zero;
        _direction = Vector2.one.normalized;
    }

    private void FixedUpdate()
    {
        transform.Translate(_direction * _speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _onBallHitPlayer?.Raise();

            HandlePaddleHit(collision);

            _speed *= GameManager.Instance.SpeedMultiplier;

        }

        else if (collision.gameObject.CompareTag("DeathWall"))
        {
            _direction.y = -_direction.y;
        }
    }

    private const float MAX_Y = 1.0f;
    private void HandlePaddleHit(Collision2D paddleCollision)
    {
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