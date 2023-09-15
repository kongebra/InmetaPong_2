using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScoresManager : MonoBehaviour
{
    private string filePath;
    public List<PlayerScoreData> highScores = new List<PlayerScoreData>();

    private void Awake()
    {
        filePath = Path.Combine("./highScoresnew.json");

        LoadScores();
    }

    public void DeleteHighScoresFile()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("High scores file deleted.");

            // Optionally, you can also clear the scores from memory
            highScores.Clear();
        }
        else
        {
            Debug.LogWarning("High scores file not found.");
        }
    }

    public void SaveScores()
    {
        string json = JsonUtility.ToJson(new Wrapper<PlayerScoreData> { items = highScores });
        File.WriteAllText(filePath, json);
    }

    public void LoadScores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            highScores = JsonUtility.FromJson<Wrapper<PlayerScoreData>>(json).items;
        }
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> items;
    }
}