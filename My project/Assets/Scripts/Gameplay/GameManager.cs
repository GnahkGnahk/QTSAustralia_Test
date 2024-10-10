using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

    [SerializeField] float radius, timeSpawn;

    [HideInInspector] public bool isGamePaused = false;

    PlayerManager playerManager_Ins;

    List<ObstaclePoolItemA> listObsA = new();
    List<ObstaclePoolItemB> listObsB = new();

    float timer = 0f;

    private void Start()
    {
        playerManager_Ins = PlayerManager.Instance;
        Spawn();
    }

    private void Update()
    {
        SpawnObjectWithTime(timeSpawn);

        UpdateDestinationForObs();

    }

    void SpawnObjectWithTime(float timeSpawn)
    {
        timer += Time.deltaTime;

        if (timer >= timeSpawn)
        {
            Spawn();

            timer = 0f;
        }        
    }

    void Spawn()
    {

        Vector3 spawnPosition = GetValidSpawnPosition();
        ObstaclePoolItemA obsA = PoolSystem.GetItem<ObstaclePoolItemA>(transform, spawnPosition);
        obsA.Setup();
        listObsA.Add(obsA);
        Debug.Log(spawnPosition);

        spawnPosition = GetValidSpawnPosition();
        ObstaclePoolItemB obsB = PoolSystem.GetItem<ObstaclePoolItemB>(transform, spawnPosition);
        obsB.Setup();
        listObsB.Add(obsB);
        Debug.Log(spawnPosition);
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
    Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPos;
        int attempts = 0;
        const int maxAttempts = 30;

        do
        {
            Vector2 randomCircle = Random.insideUnitCircle * radius;
            spawnPos = playerManager_Ins.transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            spawnPos = ClampToPlaneBounds(spawnPos);

            attempts++;
            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Không th? tìm v? trí spawn h?p l? sau " + maxAttempts + " l?n th?.");
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
        // Ki?m tra xem v? trí có n?m trong bán kính spawn và trong plane không
        return Vector3.Distance(position, playerManager_Ins.transform.position) <= radius &&
               Mathf.Abs(position.x) <= planeBounds.x / 2 &&
               Mathf.Abs(position.y) <= planeBounds.y / 2;
    }
}
