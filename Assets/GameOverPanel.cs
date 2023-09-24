using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _finalScoreText;

    [SerializeField]
    private GameObject _panel;

    private void Start()
    {
        _panel.SetActive(false);
    }

    public void ShowPanel()
    {
        _finalScoreText.text = "Final Score: " + GameManager.Instance.Score.ToString();
        _panel.SetActive(true);
    }
}
