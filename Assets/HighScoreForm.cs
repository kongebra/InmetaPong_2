using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreForm : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_InputField _nameInput;
    [SerializeField]
    private TMPro.TMP_InputField _phoneInput;

    public void OnSubmit()
    {
        if (_nameInput.text.Length == 0 || _phoneInput.text.Length == 0)
        {
            return;
        }

        var playerScore = new PlayerScoreData()
        {
            playerName = _nameInput.text,
            phoneNumber = _phoneInput.text,
            score = GameManager.Instance.Score,
        };

        Debug.Log($"Score: {playerScore.playerName} - {playerScore.phoneNumber} - {playerScore.score}");

        var highScorePlayer = HighScoresManager.Instance.highScores.Find(x =>
            x.playerName == _nameInput.text &&
            x.phoneNumber == _phoneInput.text);

        if (highScorePlayer != null)
        {
            if (playerScore.score > highScorePlayer.score)
            {
                highScorePlayer.score = playerScore.score;
            }
        }
        else
        {
            HighScoresManager.Instance.highScores.Add(playerScore);
        }

        HighScoresManager.Instance.highScores = HighScoresManager.Instance.highScores.OrderByDescending(x => x.score).ToList();
        HighScoresManager.Instance.SaveScores();

    }
}
