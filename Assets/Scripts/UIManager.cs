using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Texts")]
    [SerializeField]
    private TMPro.TextMeshProUGUI _scoreText;
    private void ShowScoreText() => _scoreText.gameObject.SetActive(true);
    private void HideScoreText() => _scoreText.gameObject.SetActive(false);
    [SerializeField]
    private TMPro.TextMeshProUGUI _gameOverScoreText;
    [SerializeField]
    private TMPro.TextMeshProUGUI _pauseScoreText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ShowHighscorePanel();
        HideSubmitFormPanel();
        HideGameOverPanel();
        HidePausePanel();

        ShowBackdrop();
        HideScoreText();
    }

    [Header("Backdrop")]
    [SerializeField]
    private GameObject _backdrop;
    private void ShowBackdrop() => _backdrop.SetActive(true);
    private void HideBackdrop() => _backdrop.SetActive(false);

    [Header("Panels")]
    #region Pause Panel
    [SerializeField]
    private GameObject _pausePanel;
    private void ShowPausePanel() => _pausePanel.SetActive(true);
    private void HidePausePanel() => _pausePanel.SetActive(false);
    #endregion

    #region Highscore Panel
    [SerializeField]
    private GameObject _highscorePanel;
    private void ShowHighscorePanel() => _highscorePanel.SetActive(true);
    private void HideHighscorePanel() => _highscorePanel.SetActive(false);
    #endregion

    #region Submit Form Panel
    [SerializeField]
    private GameObject _submitFormPanel;
    private void ShowSubmitFormPanel() => _submitFormPanel.SetActive(true);
    private void HideSubmitFormPanel() => _submitFormPanel.SetActive(false);
    #endregion

    #region Game Over Panel
    [SerializeField]
    private GameObject _gameOverPanel;
    private void ShowGameOverPanel() => _gameOverPanel.SetActive(true);
    private void HideGameOverPanel() => _gameOverPanel.SetActive(false);
    #endregion

    public void HandleGameOver()
    {
        ShowGameOverPanel();
        HideHighscorePanel();
        HideSubmitFormPanel();
        HidePausePanel();

        ShowBackdrop();
        HideScoreText();
    }

    public void HandleGameStart()
    {
        HideGameOverPanel();
        HideHighscorePanel();
        HideSubmitFormPanel();
        HidePausePanel();

        HideBackdrop();
        ShowScoreText();
    }

    public void ShowForm()
    {
        ShowSubmitFormPanel();
        HideHighscorePanel();
        HideGameOverPanel();
        HidePausePanel();

        ShowBackdrop();
        HideScoreText();
    }

    public void HandleScoreSubmitted()
    {
        HideSubmitFormPanel();
        ShowHighscorePanel();
        HideGameOverPanel();
        HidePausePanel();

        ShowBackdrop();
        HideScoreText();
    }

    public void UpdateScoreText()
    {
        var text = $"Score: {GameManager.Instance.Score}";

        _gameOverScoreText.text = text;
        _scoreText.text = text;
        _pauseScoreText.text = text;
    }

    public void HandlePauseGame()
    {
        HideGameOverPanel();
        HideHighscorePanel();
        HideSubmitFormPanel();
        ShowPausePanel();

        ShowBackdrop();
        HideScoreText();
    }

    public void HandleUnpauseGame()
    {
        HideGameOverPanel();
        HideHighscorePanel();
        HideSubmitFormPanel();
        HidePausePanel();

        HideBackdrop();
        ShowScoreText();
    }
}
