using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScene : MonoBehaviour
{
    [SerializeField] Text endGameText;
    private void Start()
    {
        Debug.Log(GameManager.Instance == null);
        endGameText.text = "Thank you for playing\n\n" + 
            (GameManager.Instance.IsWin() ? "You win!" : "You lose!") +
            "\nScore: " + GameManager.Instance.playerScore;
    }

    public void OnClickRestartGame()
    {
        LoadSceneManager.Instance.LoadScene(SceneGame.GamePlayScene);
        GameManager.Instance.RestatrGame();
    }
}
