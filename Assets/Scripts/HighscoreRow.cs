using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreRow : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _nameText;
    [SerializeField]
    private TMPro.TextMeshProUGUI _scoreText;

    [SerializeField]
    private PlayerScoreData _playerScoreData;
    public PlayerScoreData Data
    {
        get { return _playerScoreData; }
    }

    private void Awake()
    {
        _nameText.text = "AAA";
        _scoreText.text = "0";
    }

    public void SetData(PlayerScoreData data)
    {
        _playerScoreData = data;
    }

    public void RenderTexts()
    {
        _nameText.text = _playerScoreData.playerName;
        _scoreText.text = _playerScoreData.score.ToString();
    }
}
