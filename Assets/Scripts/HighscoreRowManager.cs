using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class HighscoreRowManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _highscoreRowPrefab;

    private string filePath;

    private List<PlayerScoreData> _playerScores = new();
    private List<HighscoreRow> _highscoreRows = new();

    private void Awake()
    {
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
                playerName = "AAA",
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
        // Add score
        _playerScores.Add(data);

        // Sort scores
        SortScores();
        // Save scores
        SaveScores();

        // Render top 10
        RenderTop10();
    }

#if UNITY_EDITOR
    [ContextMenu("Debug: Add Random Score")]
    public void DebugAddRandomScore()
    {
        var score = Random.Range(0, 100);
        Debug.Log($"Adding random score: {score}.");

        AddScore(new PlayerScoreData()
        {
            playerName = "DEBUG",
            score = score,
        });
    }
#endif

    public void DeleteHighScoresFile()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("High scores file deleted.");

            // Optionally, you can also clear the scores from memory
            _playerScores.Clear();
        }
        else
        {
            Debug.LogWarning("High scores file not found.");
        }
    }

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
