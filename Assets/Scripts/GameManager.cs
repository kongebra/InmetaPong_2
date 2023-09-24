using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Ball Settings")]
    [SerializeField]
    private GameObject _ballPrefab;
    [SerializeField]
    private BoxCollider2D _bounds;

    [Header("UI")]
    [SerializeField]
    private TMPro.TextMeshProUGUI _scoreText;

    [Header("Game Settings")]
    [SerializeField]
    private int _score = 0;

    public int Score => _score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        SpawnBall();

        _score = 0;
        UpdateScoreText();
    }

    private void SpawnBall()
    {
        var ballGo = Instantiate(_ballPrefab);
        ballGo.transform.position = Vector3.zero;

        var ball = ballGo.GetComponent<BallControllerV2>();

        ball.SetBounds(_bounds);
        ball.Init();
    }

    private void Update()
    {

    }

    public void ResetGame()
    {
        _score = 0;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        _scoreText.text = "Score: " + _score.ToString();
    }

    public void HandleOnBallHitPlayer()
    {
        _score++;
        UpdateScoreText();
    }

    public void GameOver()
    {

    }
}
