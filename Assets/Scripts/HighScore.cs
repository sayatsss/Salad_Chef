using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class HighScore
{
    [System.Serializable]
    public class Score
    {
        public int value;
        public int player;
    }

    [System.Serializable]
    public class HighScoreContainer
    {
        public  List<Score> scoreList = new List<Score>();
    }

    public static HighScoreContainer _scores = new HighScoreContainer();

    private static string fileName = "Score.json";

    public static void Add(int score, int player)
    {
        _scores.scoreList.Add(new Score() { value = score, player = player });
        _scores.scoreList = _scores.scoreList.OrderBy(x => x.value).ToList();
        Save();
    }

    public static void Save()
    {
        string json = JsonUtility.ToJson(_scores);
        Debug.Log("Saving at: " + Application.persistentDataPath + "/" + fileName + "\nJson: " + json);
        File.WriteAllText(Application.persistentDataPath + "/" + fileName, json);
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/" + fileName))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/" + fileName);
            _scores = JsonUtility.FromJson<HighScoreContainer>(json);
            Debug.Log("Loading from: " + Application.persistentDataPath + "/" + fileName + "\nJson: " + json);
        }
    }

    public static List<Score> GetTop10()
    {
        return _scores.scoreList.GetRange(0, _scores.scoreList.Count > 10 ? 10 : _scores.scoreList.Count);
    }

}
