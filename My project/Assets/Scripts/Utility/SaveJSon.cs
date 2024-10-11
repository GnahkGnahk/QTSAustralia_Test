using UnityEngine;
using System.IO;

public class SaveJSon : MonoBehaviour {
    private string filePath;

    private void Start()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, "score.json");
        Debug.Log("File path: " + filePath);
    }

    public void SaveScore(int score)
    {
        string json = "{\"Score\": " + score + "}";

        File.WriteAllText(filePath, json);
        Debug.Log("Score saved: " + json);
    }

    public int LoadScore()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log("Score loaded: " + json);

            int score = int.Parse(json.Substring(json.IndexOf(":") + 1).Trim().TrimEnd('}'));
            return score;
        }
        else
        {
            return 0;
        }
    }
}

