using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed;
    private Vector2 direction;
    //public GameObject gameOverPanel; // Assuming you have a UI panel to show game over
    public GameObject _canvasGO;
    private Canvas _canvas;
    private Canvas _gameOverPanelCanvas;
    public int score = 0;
    public TextMeshProUGUI scoreText; // UI Text to display score

    private float maxY = 1.0f;
    private Vector3 originalPos;
    private float startSpeed;
    private AudioSource _audioSource;

    [Header("Game Events")]
    [SerializeField]
    private GameEvent _onBallHitPlayer;
    [SerializeField]
    private GameEvent _onBallHitDeathWall;

    void Awake()
    {
        originalPos = transform.position;
        startSpeed = speed;
        _audioSource = GetComponent<AudioSource>();
        _gameOverPanelCanvas = GetComponent<Canvas>();
        _canvas = _canvasGO.GetComponent<Canvas>();
    }

    void Start()
    {
        // Start by moving the ball to the right and slightly upwards
        direction = new Vector2(1, 1).normalized;
        SetScoreText();
        //gameOverPanel.SetActive(false);
        _canvas.enabled = false;
    }

    public void ResetScore()
    {
        score = 0;
        SetScoreText();
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void SetScoreText()
    {
        scoreText.SetText("Score: " + score);
    }

    public void ResetBall()
    {
        speed = startSpeed;
        transform.position = originalPos;
        direction = new Vector2(1, 1).normalized;
    }

    private void HandlePaddleHit(Collision2D paddleCollision)
    {
        // Calculate where the ball hit the paddle (-1 = bottom, 1 = top)
        float hitFactor = (transform.position.y - paddleCollision.transform.position.y) / paddleCollision.collider.bounds.size.y;

        // Clamp the hitFactor to prevent excessive vertical movement
        hitFactor = Mathf.Clamp(hitFactor, -0.7071f, 0.7071f);  // This limits the vertical reflection to approximately 45 degrees

        // Adjust the y-direction based on where the ball hit the paddle
        direction.y += hitFactor;

        var positive = direction.y > 0;
        var negative = direction.y < 0;

        if (positive && direction.y > maxY)
        {
            direction.y = maxY;
        }

        if (negative && direction.y < -maxY)
        {
            direction.y = -maxY;
        }

        // Normalize the direction to ensure consistent speed
        direction = direction.normalized;

        // Since the ball hit the player, reverse the x direction
        direction.x = -direction.x;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the ball hits the player, bounce and increase score
        if (collision.gameObject.name == "Player")
        {
            _onBallHitPlayer?.Raise();

            //_gameOverPanelCanvas.enabled = true;
            _audioSource.Play();
            HandlePaddleHit(collision);
            speed *= 1.05f;
            //direction = new Vector2(-direction.x, direction.y);
            score++;
            SetScoreText();
        }
        else if (collision.gameObject.tag == "Wall")
        {
            // If it's not the right wall, bounce off normally
            direction = Vector2.Reflect(direction, collision.contacts[0].normal);
            _audioSource.Play();
        }
        else
        {
            _onBallHitDeathWall?.Raise();

            // Ball passed the player, game over
            //gameOverPanel.SetActive(true);
            _canvas.enabled = true;
            speed = 0;

            var playerUIInteraction = GameObject.Find("GameOverPanel").GetComponent<PlayerUIInteraction>();

            if (playerUIInteraction.newPlayer)
            {
                playerUIInteraction.DisplayHighScores();
            }
            else
            {
                playerUIInteraction.AddToHighScore();
            }
        }
    }
}