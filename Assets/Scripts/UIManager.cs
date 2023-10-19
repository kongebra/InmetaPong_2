using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

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
    }

    [Header("Panels")]
    #region Highscore Panel
    [SerializeField]
    private GameObject _highscorePanel;
    public void ShowHighscorePanel() => _highscorePanel.SetActive(true);
    public void HideHighscorePanel() => _highscorePanel.SetActive(false);
    #endregion

    #region Submit Form Panel
    [SerializeField]
    private GameObject _submitFormPanel;
    public void ShowSubmitFormPanel() => _submitFormPanel.SetActive(true);
    public void HideSubmitFormPanel() => _submitFormPanel.SetActive(false);
    #endregion

    #region Game Over Panel
    [SerializeField]
    private GameObject _gameOverPanel;
    public void ShowGameOverPanel() => _gameOverPanel.SetActive(true);
    public void HideGameOverPanel() => _gameOverPanel.SetActive(false);
    #endregion

    public void HandleOnGameOver()
    {
        ShowGameOverPanel();
        HideHighscorePanel();
        HideSubmitFormPanel();
    }

    public void HandleOnPlayButtonClicked()
    {
        HideGameOverPanel();
        HideHighscorePanel();
        HideSubmitFormPanel();
    }

    public void HandleOnSubmitScoreButtonClicked()
    {
        ShowSubmitFormPanel();
        HideHighscorePanel();
        HideGameOverPanel();
    }

    public void UpdateScoreText()
    {
        _scoreText.SetText($"Score: {GameManager.Instance.Score}");
    }
}
