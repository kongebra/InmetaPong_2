using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private float _ballSpeedCoefficient = 1.10f;
    public float SpeedMultiplier => _ballSpeedCoefficient;

    [SerializeField]
    private int _currentScore = 0;
    public int Score => _currentScore;

    [Header("Game Events")]
    [SerializeField]
    private GameEvent _updateScoreTextEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void OnBallHitPlayerEventHandler()
    {
        _currentScore++;
        _updateScoreTextEvent?.Raise();
    }
}
