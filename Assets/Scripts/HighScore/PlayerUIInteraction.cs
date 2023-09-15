using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIInteraction : MonoBehaviour
{
    public HighScoresManager scoresManager;
    public BallController ballController;
    public GameObject gameOverPanel;
    public TMP_InputField nameInput;
    public TMP_InputField phoneInput;
    public TextMeshProUGUI highScoresText;
    public Button enterHighScoreBtn;
    public Button tryAgainBtn;

    private PlayerScoreData currentPlayer;
    public bool newPlayer = true;

    public void Awake()
    {
        tryAgainBtn.gameObject.SetActive(false);
    }

    public void StartNewGame()
    {
        if (currentPlayer == null)
        {
            currentPlayer = new PlayerScoreData();
        }

        currentPlayer.playerName = nameInput.text;
        currentPlayer.phoneNumber = phoneInput.text;
        currentPlayer.score = ballController.score;


        // Restart game logic here...
    }


    public void AddToHighScore()
    {
        if (currentPlayer == null)
        {
            currentPlayer = new PlayerScoreData();
        }

        if (nameInput.text.Length == 0 || phoneInput.text.Length == 0)
        {
            return;
        }

        currentPlayer.playerName = nameInput.text;
        currentPlayer.phoneNumber = phoneInput.text;
        currentPlayer.score = ballController.score;


        var highScorePlayer = scoresManager.highScores.Find(x =>
            x.playerName == nameInput.text &&
            x.phoneNumber == phoneInput.text);

        if (highScorePlayer != null)
        {
            if (ballController.score > highScorePlayer.score)
            {
                highScorePlayer.score = ballController.score;
            }
        }
        else
        {
            scoresManager.highScores.Add(currentPlayer);
        }

        scoresManager.highScores = scoresManager.highScores.OrderByDescending(x => x.score).ToList();
        scoresManager.SaveScores();

        DisplayHighScores();

        enterHighScoreBtn.enabled = false;
        tryAgainBtn.gameObject.SetActive(true);

    }


    public void DisplayHighScores()
    {
        highScoresText.text = "High Scores:\n";
        var count = 0;
        foreach (var playerData in scoresManager.highScores)
        {
            highScoresText.text += $"{playerData.playerName} - {playerData.score}\n";
            count++;

            if (count >= 10)
            {
                break;
            }
        }
    }

    public void TryAgain()
    {
        newPlayer = false;
        // Restart game logic here...

        gameOverPanel.SetActive(false);

        enterHighScoreBtn.enabled = false;

        nameInput.gameObject.SetActive(false);
        phoneInput.gameObject.SetActive(false);
        enterHighScoreBtn.gameObject.SetActive(false);

        ballController.ResetBall();
        ballController.ResetScore();
        tryAgainBtn.gameObject.SetActive(true);

    }

    public void NewPlayer()
    {
        newPlayer = true;

        currentPlayer = null;

        nameInput.text = "";
        phoneInput.text ="";

        nameInput.gameObject.SetActive(true);
        phoneInput.gameObject.SetActive(true);
        enterHighScoreBtn.gameObject.SetActive(true);
        tryAgainBtn.gameObject.SetActive(false);


        gameOverPanel.SetActive(false);

        ballController.ResetBall();
        ballController.ResetScore();
    }
}