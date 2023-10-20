using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class HighscoreRowManager : MonoBehaviour
{
    public static HighscoreRowManager Instance;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject _highscoreRowPrefab;

    [Header("Game Events")]
    [SerializeField]
    private GameEvent _onScoreSubmitted;

    private string filePath;

    private List<PlayerScoreData> _playerScores = new();
    private List<HighscoreRow> _highscoreRows = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        filePath = Path.Combine("./highScoresnew.json");

        LoadScores();

        if (_playerScores.Count < 10)
        {
            Debug.Log($"Not enough scores, filling with default scores. COUNT: {_playerScores.Count}");
            FillDefaultScores();
        }

        RenderTop10();
    }

    private void FillDefaultScores()
    {
        var fillAmount = 10 - _playerScores.Count;
        Debug.Log($"Filling with {fillAmount} default scores.");

        for (int i = 0; i < fillAmount; i++)
        {
            _playerScores.Add(new PlayerScoreData()
            {
                playerName = "---",
                score = 0
            });
        }

        SortScores();
    }

    private void SortScores()
    {
        _playerScores.Sort((a, b) => b.score.CompareTo(a.score));
    }

    private void RenderTop10()
    {
        // remove all children
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // render top 10
        for (int i = 0; i < 10; i++)
        {
            var row = Instantiate(_highscoreRowPrefab, transform).GetComponent<HighscoreRow>();
            row.SetData(_playerScores[i]);
            row.RenderTexts();
        }
    }

    public void AddScore(PlayerScoreData data)
    {
        // Handle duplicate emails
        var index = _playerScores.FindIndex(x => x.email == data.email);
        if (index >= 0)
        {
            var prevScore = _playerScores[index].score;
            // Check if we want to update the score
            if (prevScore < data.score)
            {
                // Update score
                _playerScores[index] = data;
            }
        }
        else
        {
            // New player/email, add score
            _playerScores.Add(data);
        }

        // Sort scores
        SortScores();
        // Save scores
        SaveScores();

        _onScoreSubmitted?.Raise();

        // Render top 10
        RenderTop10();
    }

#if UNITY_EDITOR
    [ContextMenu("Debug: Add Random Score")]
    public void DebugAddRandomScore()
    {
        var names = new string[] {
            "John",
            "Sally",
            "Bob",
            "Alice",
            "Joe",
            "Jane",
            "Bill",
            "Jill",
            "Jack",
            "Jen",
            "Jim",
            "Karl",
            "Fred",
            "Carl Johan",
            "Frank von Fristelsen",
            "Sven Svensson",
        };

        var name = names[Random.Range(0, names.Length)];
        var score = Random.Range(0, 100);

        AddScore(new PlayerScoreData()
        {
            playerName = name,
            score = score,
        });
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Debug: Delete High Scores File")]
    public void DeleteHighScoresFile()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("High scores file deleted.");
        }
        else
        {
            Debug.LogWarning("High scores file not found.");
        }

        _playerScores.Clear();
        FillDefaultScores();
        RenderTop10();
    }
#endif

    public void SaveScores()
    {
        string json = JsonUtility.ToJson(new Wrapper<PlayerScoreData> { items = _playerScores });
        File.WriteAllText(filePath, json);
    }

    public void LoadScores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            _playerScores = JsonUtility.FromJson<Wrapper<PlayerScoreData>>(json).items;
        }
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> items;
    }
}
