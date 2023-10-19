using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Texts")]
    [SerializeField]
    private TMPro.TextMeshProUGUI _scoreText;

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

        _backdrop.SetActive(true);
        _scoreText.gameObject.SetActive(false);
    }

    [Header("Backdrop")]
    [SerializeField]
    private GameObject _backdrop;

    [Header("Panels")]
    #region Highscore Panel
    [Header("Highscore Panel")]
    [SerializeField]
    private GameObject _highscorePanel;
    public void ShowHighscorePanel() => _highscorePanel.SetActive(true);
    public void HideHighscorePanel() => _highscorePanel.SetActive(false);
    #endregion

    #region Submit Form Panel
    [Header("Submit Form Panel")]
    [SerializeField]
    private GameObject _submitFormPanel;
    public void ShowSubmitFormPanel() => _submitFormPanel.SetActive(true);
    public void HideSubmitFormPanel() => _submitFormPanel.SetActive(false);
    #endregion

    #region Game Over Panel
    [Header("Game Over Panel")]
    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private TMPro.TextMeshProUGUI _gameOverScoreText;
    public void ShowGameOverPanel() => _gameOverPanel.SetActive(true);
    public void HideGameOverPanel() => _gameOverPanel.SetActive(false);
    #endregion

    public void HandleGameOver()
    {
        ShowGameOverPanel();
        HideHighscorePanel();
        HideSubmitFormPanel();

        _backdrop.SetActive(true);
        _scoreText.gameObject.SetActive(false);
    }

    public void HandleGameStart()
    {
        HideGameOverPanel();
        HideHighscorePanel();
        HideSubmitFormPanel();

        _backdrop.SetActive(false);
        _scoreText.gameObject.SetActive(true);
    }

    public void ShowForm()
    {
        ShowSubmitFormPanel();
        HideHighscorePanel();
        HideGameOverPanel();

        _backdrop.SetActive(true);
        _scoreText.gameObject.SetActive(false);
    }


    public void UpdateScoreText()
    {
        _gameOverScoreText.text = $"Score: {GameManager.Instance.Score}";
        _scoreText.SetText($"Score: {GameManager.Instance.Score}");
    }
}
