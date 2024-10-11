using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    public int playerScore = 0;
    public float playerHP_total = 100;
    float playerHP_current = 10;
    [SerializeField][Tooltip("Second")] float gameTimerTotal, timeSpawnObs, timeSpawnPoint;
    [SerializeField] float radius;

    [HideInInspector] public bool isGamePaused = false;

    LoadSceneManager sceneManager_Ins;
    PlayerManager playerManager_Ins;
    UIManager UIManager_Ins;

    List<ObstaclePoolItemA> listObsA = new();
    List<ObstaclePoolItemB> listObsB = new();
    List<ObstaclePoolItemPoint> listObsPoint = new();

    float timerSpawn = 0f;

    float timerGame = 0f;
    bool isEndGame = false;


    [HideInInspector] public int lastGameScore = 0;

    private SaveJSon saveJSon;

    private void Start()
    {
        playerManager_Ins = PlayerManager.Instance;
        UIManager_Ins = UIManager.Instance;
        sceneManager_Ins = LoadSceneManager.Instance;

        FindJSon();

        RestatrGame();


    }

    private void Update()
    {
        if (isEndGame) return;

        if (timerGame <= 0 || playerHP_current <= 0)
        {
            isEndGame = true;

            sceneManager_Ins.LoadScene(SceneGame.EndGameScene);

            saveJSon.SaveScore(playerScore);
        }

        SpawnObjectWithTime(timeSpawnObs, SpawnType.OBSTACLE_A);
        SpawnObjectWithTime(timeSpawnObs, SpawnType.OBSTACLE_B);
        SpawnObjectWithTime(timeSpawnPoint, SpawnType.OBSTACLE_POINT);

        UpdateDestinationForObs();

        timerGame -= Time.deltaTime;
        UIManager_Ins.UpdateText(UIManager_Ins.timerTxt, "Time left:" + Mathf.Round(timerGame));
    }
    public void SpawnObjectWithTime(float timeSpawn, SpawnType spawnType)
    {
        timerSpawn += Time.deltaTime;
        if (timerSpawn >= timeSpawn)
        {
            Spawn(spawnType);
            timerSpawn = 0f;
        }
    }

    private void Spawn(SpawnType spawnType)
    {
        Vector3 spawnPosition = GetValidSpawnPosition();

        switch (spawnType)
        {
            case SpawnType.OBSTACLE_POINT:
                ObstaclePoolItemPoint obsPoint = PoolSystem.GetItem<ObstaclePoolItemPoint>(transform, spawnPosition);
                obsPoint.Setup();
                listObsPoint.Add(obsPoint);
                break;
            case SpawnType.OBSTACLE_A:
                ObstaclePoolItemA obsA = PoolSystem.GetItem<ObstaclePoolItemA>(transform, spawnPosition);
                obsA.Setup();
                listObsA.Add(obsA);
                break;
            case SpawnType.OBSTACLE_B:
                ObstaclePoolItemB obsB = PoolSystem.GetItem<ObstaclePoolItemB>(transform, spawnPosition);
                obsB.Setup();
                listObsB.Add(obsB);
                break;
        }
    }

    void UpdateDestinationForObs()
    {
        Vector3 pos = playerManager_Ins.transform.position;
        foreach (ObstaclePoolItemA obs in listObsA)
        {
            obs.UpdateDestination(pos);
        }
        foreach (ObstaclePoolItemB obs in listObsB)
        {
            obs.UpdateDestination(pos);
        }
    }

    public void PauseGame(bool isPause = true)
    {
        if (!isPause)
        {
            Time.timeScale = 1.0f;
            return;
        }

        Time.timeScale = 0;
    }

    public void AddPoints(int points)
    {
        playerScore += points;
        UIManager_Ins.UpdateText(UIManager_Ins.scoreTxt, "Score:" + playerScore);
    }

    public void LostHP(int hpLost)
    {
        playerHP_current -= hpLost;
        UIManager_Ins.UpdateHpBarVisual(playerHP_current/ playerHP_total);
    }

    Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPos;
        int attempts = 0;
        const int maxAttempts = 30;

        do
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * radius;
            spawnPos = playerManager_Ins.transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            spawnPos = ClampToPlaneBounds(spawnPos);

            attempts++;
            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Not found after try " + maxAttempts + " times.");
                break;
            }
        } while (!IsPositionValid(spawnPos));

        return spawnPos;
    }
    Vector2 planeBounds = new Vector2(50f, 50f);
    Vector3 ClampToPlaneBounds(Vector3 position)
    {
        float halfWidth = planeBounds.x / 2;
        float halfHeight = planeBounds.y / 2;

        position.x = Mathf.Clamp(position.x, -halfWidth, halfWidth);
        position.y = Mathf.Clamp(position.y, -halfHeight, halfHeight);

        return position;
    }
    bool IsPositionValid(Vector3 position)
    {
        return Vector3.Distance(position, playerManager_Ins.transform.position) <= radius &&
               Mathf.Abs(position.x) <= planeBounds.x / 2 &&
               Mathf.Abs(position.y) <= planeBounds.y / 2;
    }

    public bool IsWin()
    {
        if (playerScore > 0)
        {
            return true;
        }
        return false;
    }

    void ReturnAllItemToPool()
    {
        foreach (var item in listObsA)
        {
            PoolSystem.ReturnItem(item);
        }
        foreach (var item in listObsB)
        {
            PoolSystem.ReturnItem(item);
        }
        foreach (var item in listObsPoint)
        {
            PoolSystem.ReturnItem(item);
        }
    }

    public void RestatrGame()
    {
        Debug.Log("Restart game");
        playerHP_current = playerHP_total;
        timerGame = gameTimerTotal;
        playerScore = 0;

        playerManager_Ins.transform.position = Vector3.zero;

        ReturnAllItemToPool();

        Spawn(SpawnType.OBSTACLE_A);
        Spawn(SpawnType.OBSTACLE_B);
        Spawn(SpawnType.OBSTACLE_POINT);

        UIManager_Ins.UpdateText(UIManager_Ins.scoreTxt, "Score:" + playerScore);
        UIManager_Ins.UpdateHpBarVisual(playerHP_current / playerHP_total);
        UIManager_Ins.UpdateText(UIManager_Ins.timerTxt, "Time left:" + Mathf.Round(timerGame));

        isEndGame = false;

        LoadLastScore();
        
    }

    void FindJSon()
    {
        saveJSon = FindObjectOfType<SaveJSon>();

        if (saveJSon == null)
        {
            Debug.LogError("SaveJSon component not found in the scene.");
        }
    }

    public int LoadLastScore()
    {
        if (!saveJSon)
        {
            FindJSon();
        }
        lastGameScore = saveJSon.LoadScore();
        //Debug.Log("Loaded Score: " + lastGameScore);
        return lastGameScore;
    }
}
