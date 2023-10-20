using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        FormInput,
        GameOver,
    }

    public static GameManager Instance;


    [Header("Game Settings")]
    [SerializeField]
    private float _ballSpeedCoefficient = 1.10f;
    public float SpeedMultiplier => _ballSpeedCoefficient;

    [Header("Game Events")]
    [SerializeField]
    private GameEvent _startGameEvent;
    [SerializeField]
    private GameEvent _pauseGameEvent;
    [SerializeField]
    private GameEvent _unpauseGameEvent;
    [SerializeField]
    private GameEvent _updateScoreTextEvent;
    [SerializeField]
    private GameEvent _gotoFormInputEvent;


    private GameState _gameState = GameState.Menu;
    public GameState State => _gameState;

    private int _currentScore = 0;
    public int Score => _currentScore;


    private PlayerInputActions _inputActions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _inputActions = new PlayerInputActions();

        _inputActions.Player.Jump.performed += ctx => HandleJumpPerformed(ctx);
        _inputActions.Player.Start.performed += ctx => HandleStartPerformed(ctx);
        _inputActions.Player.Triangle.performed += ctx => HandleTrianglePerformed(ctx);
    }

    private void HandleTrianglePerformed(InputAction.CallbackContext ctx)
    {
        switch (_gameState)
        {
            case GameState.GameOver:
                _gotoFormInputEvent?.Raise();
                _gameState = GameState.FormInput;
                break;
        }
    }

    private void HandleStartPerformed(InputAction.CallbackContext ctx)
    {
        switch (_gameState)
        {
            case GameState.Playing:
                _gameState = GameState.Paused;
                Time.timeScale = 0f;
                _pauseGameEvent?.Raise();
                break;

            case GameState.Paused:
                _gameState = GameState.Playing;
                Time.timeScale = 1f;
                _unpauseGameEvent?.Raise();
                break;
        }
    }

    private void HandleJumpPerformed(InputAction.CallbackContext ctx)
    {
        switch (_gameState)
        {
            case GameState.Menu:
            case GameState.GameOver:
                _startGameEvent?.Raise();
                _gameState = GameState.Playing;
                break;
        }
    }

    public void OnBallHitPlayerEventHandler()
    {
        _currentScore++;
        _updateScoreTextEvent?.Raise();
    }

    public void HandleGameStart()
    {
        _currentScore = 0;
        _gameState = GameState.Playing;

        _updateScoreTextEvent?.Raise();
    }

    public void HandleGameOver()
    {
        _gameState = GameState.GameOver;
    }

    public void HandleScoreSubmitted()
    {
        _gameState = GameState.Menu;
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
