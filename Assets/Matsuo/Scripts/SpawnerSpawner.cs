using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSpawner : MonoBehaviour
{
    public bool canSpawn = false;
    [SerializeField] GameObject enemySpawner;
    [SerializeField] float minX, maxX, minY, maxY, width;


    [SerializeField] int spawnerInScene = 0;
    [SerializeField] int spawnerLimit;
    public void OnSpawnerDied(EnemySpawner spawner)
    {
        spawnerInScene--;
    }


    float timer = 0;
    [SerializeField] float maxSpawnTime, minSpawnTime;
    float spawnTime;
    float GetRandomSpawnTime() => spawnTime = Random.Range(minSpawnTime, maxSpawnTime);


    [SerializeField] float firstWaveTime, secondWaveTime, thirdWaveTime;
    [SerializeField] int firstWaveSpawnInterval, secondWaveSpawnInterval, thirdWaveSpawnInterval;
    [SerializeField] TimeManager timeManager;



    void Start()
    {
        spawnTime = GetRandomSpawnTime();
    }



    void Update()
    {
        if (GameManager.Instance._isFinish)
        {
            return;
        }

        if (canSpawn)
        {
            if(spawnerInScene >= spawnerLimit)
            {
                return;
            }

            timer += Time.deltaTime;
            if (timer >= spawnTime)
            {
                timer = 0;
                spawnTime = GetRandomSpawnTime();
                EnemySpawner spawned = SpawnSpawner();

                // 時間経過でEnemySpawnerのスポーン頻度を変更
                if (timeManager.time > thirdWaveTime)
                {
                    spawned.avgSpawnInterval = thirdWaveSpawnInterval;
                }
                else if (timeManager.time > secondWaveTime)
                {
                    spawned.avgSpawnInterval = secondWaveSpawnInterval;
                }
                else if (timeManager.time > firstWaveTime)
                {
                    spawned.avgSpawnInterval = firstWaveSpawnInterval;
                }
            }
        }
    }



    EnemySpawner SpawnSpawner()
    {
        spawnerInScene ++;
        Vector2 spawnPos = GetRandomPosInRect();
        GameObject spawner = Instantiate(enemySpawner, spawnPos, Quaternion.identity);
        EnemySpawner spawned = spawner.GetComponent<EnemySpawner>();
        spawned.Init(this);
        return spawned;
    }



    // 中空四角形領域のランダムな位置を返す
    Vector2 GetRandomPosInRect()
    {
        float spawnX, spawnY;
        int edgeNum = Random.Range(0, 4);   // ４つの辺のどの辺にスポーンするか？
        switch (edgeNum)
        {
            case 0:
                spawnX = Random.Range(minX - width, minX);
                spawnY = Random.Range(minY, maxY);
                break;

            case 1:
                spawnX = Random.Range(maxX, maxX + width);
                spawnY = Random.Range(minY, maxY);
                break;

            case 2:
                spawnX = Random.Range(minX, maxX);
                spawnY = Random.Range(minY - width, minY);
                break;

            case 3:
                spawnX = Random.Range(minX, maxX);
                spawnY = Random.Range(maxY, maxY + width);
                break;
            default:
                spawnX = Random.Range(minX, maxX);
                spawnY = Random.Range(maxY, maxY + width);
                break;
        }
        Vector2 spawnPos = new Vector2(spawnX, spawnY);
        return spawnPos;
    }
}
